using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Extract;
using AMI.Core.Enums;
using AMI.Core.Extensions.Time;
using AMI.Core.Extractors;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.Serializers;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using AMI.Gif.Writers;
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
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddLogging(builder =>
                {
                    builder
                        .AddConfiguration(configuration.GetSection("Logging"))
                        .AddConsole();
                });
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IDefaultJsonSerializer, DefaultJsonSerializer>();
            services.AddScoped<IImageExtractor, ItkImageExtractor>();
            services.AddScoped<IGifImageWriter, AnimatedGifImageWriter>();
            services.AddScoped<IDefaultJsonWriter, DefaultJsonWriter>();
            services.AddSingleton<IFileSystemStrategy, FileSystemStrategy>();
            services.AddSingleton<IAppInfoFactory, AppInfoFactory>();
            services.AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>();
            services.AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>();

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(ExtractCommandHandler).GetTypeInfo().Assembly);

            // Add FluentValidation
            AssemblyScanner.FindValidatorsInAssemblyContaining<ExtractCommandValidator>().ForEach(pair => {
                // filter out validators you don't want here
                services.AddTransient(pair.InterfaceType, pair.ValidatorType);
            });

            var serviceProvider = services.BuildServiceProvider();

            Logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            Mediator = serviceProvider.GetService<IMediator>();
            Configuration = serviceProvider.GetService<IAmiConfigurationManager>();
        }

        public ILogger Logger { get; }

        public IMediator Mediator { get; }

        public IAmiConfigurationManager Configuration { get; }

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
                    await program.Execute(args, ct);
                }, ct);

                if (program.Configuration.TimeoutMilliseconds > 0)
                {
                    if (!task.Wait(program.Configuration.TimeoutMilliseconds, ct))
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

        public async Task Execute(string[] args, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation($"{this.GetMethodName()} started");

            var command = new ExtractObjectCommand();

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       command.DesiredSize = o.DesiredSize;
                       command.AmountPerAxis = o.AmountPerAxis;
                       command.SourcePath = o.SourcePath;
                       command.DestinationPath = o.DestinationPath;
                       command.Grayscale = Convert.ToBoolean(o.Grayscale);
                       command.OpenCombinedGif = Convert.ToBoolean(o.OpenCombinedGif);
                   });

            if (string.IsNullOrWhiteSpace(command.SourcePath))
            {
                throw new ArgumentNullException(nameof(command.SourcePath));
            }

            if (string.IsNullOrWhiteSpace(command.DestinationPath))
            {
                throw new ArgumentNullException(nameof(command.DestinationPath));
            }

            var result = await Mediator.Send(command, ct);

            if (command.OpenCombinedGif)
            {
                Process.Start(Path.Combine(command.DestinationPath, result.CombinedGif));
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"{this.GetMethodName()} ended after {t.ToReadableTime()}");
        }

        public async Task ExecuteTest(string[] args, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation($"{this.GetMethodName()} started");

            var command = new ExtractObjectCommand()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "SMIR.Brain.XX.O.MR_Flair.36620.mha"),
                DestinationPath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N")),
                // WatermarkSourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "watermark.png"),
                OpenCombinedGif = true
            };

            command.AxisTypes.Add(AxisType.Z);

            var result = await Mediator.Send(command, ct);

            if (Convert.ToBoolean(command.OpenCombinedGif))
            {
                Process.Start(Path.Combine(command.DestinationPath, result.CombinedGif));
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"{this.GetMethodName()} ended after {t.ToReadableTime()}");
        }
    }
}
