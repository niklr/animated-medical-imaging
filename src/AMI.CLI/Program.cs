using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Enums;
using AMI.Core.Extensions.Time;
using AMI.Core.Extractors;
using AMI.Core.Factories;
using AMI.Core.Helpers;
using AMI.Core.Models;
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
            Logger.LogInformation("AMI.Portable started");

            var input = new ExtractInput();

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       input.DesiredSize = o.DesiredSize;
                       input.AmountPerAxis = o.AmountPerAxis;
                       input.SourcePath = o.SourcePath;
                       input.DestinationPath = o.DestinationPath;
                       input.Grayscale = Convert.ToBoolean(o.Grayscale);
                       input.OpenCombinedGif = Convert.ToBoolean(o.OpenCombinedGif);
                   });

            if (string.IsNullOrWhiteSpace(input.SourcePath))
            {
                throw new ArgumentNullException(nameof(input.SourcePath));
            }

            if (string.IsNullOrWhiteSpace(input.DestinationPath))
            {
                throw new ArgumentNullException(nameof(input.DestinationPath));
            }

            var output = await ImageService.ExtractAsync(input, ct);

            if (input.OpenCombinedGif)
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo(Path.Combine(input.DestinationPath, output.CombinedGif))
                    {
                        UseShellExecute = true
                    }
                };
                p.Start();
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"AMI.Portable ended after {t.ToReadableTime()}");
        }

        private async Task ExecuteTest(string[] args, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation("AMI.Portable started");

            var input = new ExtractInput()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "data", "SMIR.Brain.XX.O.MR_Flair.36620.mha"),
                DestinationPath = Path.Combine(FileSystemHelper.BuildCurrentPath(), "temp", Guid.NewGuid().ToString("N")),
                OpenCombinedGif = true
            };

            input.AxisTypes.Add(AxisType.Z);

            var output = await ImageService.ExtractAsync(input, ct);

            if (Convert.ToBoolean(input.OpenCombinedGif))
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo(Path.Combine(input.DestinationPath, output.CombinedGif))
                    {
                        UseShellExecute = true
                    }
                };
                p.Start();
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"AMI.Portable ended after {t.ToReadableTime()}");
        }
    }
}
