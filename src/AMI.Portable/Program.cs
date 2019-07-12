using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Behaviors;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Extensions.Time;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.IO.Builders;
using AMI.Core.IO.Extractors;
using AMI.Core.IO.Serializers;
using AMI.Core.IO.Writers;
using AMI.Core.Mappers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Domain.Enums;
using AMI.Gif.Writers;
using AMI.Infrastructure.IO.Builders;
using AMI.Infrastructure.IO.Writers;
using AMI.Infrastructure.Services;
using AMI.Infrastructure.Strategies;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
using CommandLine;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Portable
{
    public class Program
    {
        public Program()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<AppOptions>(configuration.GetSection("AppOptions"));
            services.AddLogging(builder =>
                {
                    builder
                        .AddConfiguration(configuration.GetSection("Logging"))
                        .AddConsole();
                });
            services.AddScoped<IGatewayService, GatewayService>();
            services.AddScoped<IIdGenService, IdGenService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IImageExtractor, ItkImageExtractor>();
            services.AddScoped<IGifImageWriter, AnimatedGifImageWriter>();
            services.AddScoped<IDefaultJsonWriter, DefaultJsonWriter>();
            services.AddSingleton<IAmiUnitOfWork, MockUnitOfWork>();
            services.AddSingleton<IApplicationConstants, ApplicationConstants>();
            services.AddSingleton<IFileSystemStrategy, FileSystemStrategy>();
            services.AddSingleton<IFileExtensionMapper, FileExtensionMapper>();
            services.AddSingleton<IAppInfoFactory, AppInfoFactory>();
            services.AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IGatewayGroupNameBuilder, GatewayGroupNameBuilder>();
            services.AddSingleton<IGatewayObserverService, GatewayObserverService>();
            services.AddTransient<IDefaultJsonSerializer, DefaultJsonSerializer>();

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(ProcessCommandHandler).GetTypeInfo().Assembly);

            // Add FluentValidation
            AssemblyScanner.FindValidatorsInAssemblyContaining<ProcessCommandValidator>().ForEach(pair =>
            {
                // filter out validators that are not needed here
                services.AddTransient(pair.InterfaceType, pair.ValidatorType);
            });

            var serviceProvider = services.BuildServiceProvider();

            Logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            Mediator = serviceProvider.GetService<IMediator>();
            Configuration = serviceProvider.GetService<IAppConfiguration>();
        }

        public ILogger Logger { get; }

        public IMediator Mediator { get; }

        public IAppConfiguration Configuration { get; }

        public static int Main(string[] args)
        {
            try
            {
                var program = new Program();
                var cts = new CancellationTokenSource();
                var ct = cts.Token;

                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                var task = Task.Run(async () =>
                {
                    await program.ExecuteAsync(args, ct);
                }, ct);

                if (program.Configuration.Options.TimeoutMilliseconds > 0)
                {
                    if (!task.Wait(program.Configuration.Options.TimeoutMilliseconds, ct))
                    {
                        string timeoutMessage = "Process timed out";
                        program.Logger.LogInformation(timeoutMessage);
                        cts.Cancel();
                        throw new Exception(timeoutMessage);
                    }
                }
                else
                {
                    task.Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Environment.ExitCode = 1;
            }

            return Environment.ExitCode;
        }

        public async Task ExecuteAsync(string[] args, CancellationToken ct)
        {
            var command = new ProcessPathCommand();
            bool openCombinedGif = false;

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       command.OutputSize = o.OutputSize;
                       command.AmountPerAxis = o.AmountPerAxis;
                       command.SourcePath = o.SourcePath;
                       command.DestinationPath = o.DestinationPath;
                       command.Grayscale = Convert.ToBoolean(o.Grayscale);
                       openCombinedGif = Convert.ToBoolean(o.OpenCombinedGif);
                   });

            if (args.Contains("--help") || args.Contains("--version"))
            {
                await Task.CompletedTask;
            }
            else
            {
                await ExecuteCommandAsync(command, ct, openCombinedGif);
            }
        }

        public async Task ExecuteTestAsync(string[] args, CancellationToken ct)
        {
            var command = new ProcessPathCommand()
            {
                AmountPerAxis = 10,
                OutputSize = 250,
                SourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "SMIR.Brain.XX.O.MR_Flair.36620.mha"),
                DestinationPath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N")),
                // WatermarkSourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "watermark.png")
            };

            command.AxisTypes.Add(AxisType.Z);

            await ExecuteCommandAsync(command, ct, true);
        }

        private async Task ExecuteCommandAsync(ProcessPathCommand command, CancellationToken ct, bool openCombinedGif = false)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation($"{this.GetMethodName()} started");

            var result = await Mediator.Send(command, ct);

            if (openCombinedGif)
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo(Path.Combine(command.DestinationPath, result.CombinedGif))
                    {
                        UseShellExecute = true
                    }
                };
                p.Start();
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"{this.GetMethodName()} ended after {t.ToReadableTime()}");
        }
    }
}
