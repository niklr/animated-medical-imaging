using System;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.Reflection;

namespace AMI.CLI
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        public Program()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<AppSettings>(configuration.GetSection("AppSettings"))
                .AddLogging(builder =>
                {
                    builder
                        .AddConfiguration(configuration.GetSection("Logging"))
                        .AddConsole();
                })
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IDefaultJsonSerializer, DefaultJsonSerializer>()
                .AddScoped<IImageExtractor, ItkImageExtractor>()
                .AddScoped<IGifImageWriter, AnimatedGifImageWriter>()
                .AddScoped<IDefaultJsonWriter, DefaultJsonWriter>()
                .AddSingleton<IFileSystemStrategy, FileSystemStrategy>()
                .AddSingleton<IAppInfoFactory, AppInfoFactory>()
                .AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>()
                .AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>()
                .BuildServiceProvider();

            Logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            ImageService = serviceProvider.GetService<IImageService>();
            Configuration = serviceProvider.GetService<IAmiConfigurationManager>();
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets the image service.
        /// </summary>
        public IImageService ImageService { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IAmiConfigurationManager Configuration { get; }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code.</returns>
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

                var task = Task.Run(
                    async () =>
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

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// SourcePath
        /// or
        /// DestinationPath
        /// </exception>
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

            var result = await ImageService.ExtractAsync(command, ct);

            if (command.OpenCombinedGif)
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

        private async Task ExecuteTest(string[] args, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation($"{this.GetMethodName()} started");

            var command = new ExtractObjectCommand()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "SMIR.Brain.XX.O.MR_Flair.36620.mha"),
                DestinationPath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N")),
                OpenCombinedGif = true
            };

            command.AxisTypes.Add(AxisType.Z);

            var result = await ImageService.ExtractAsync(command, ct);

            if (Convert.ToBoolean(command.OpenCombinedGif))
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
