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
using AMI.Itk.Readers;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMI.Portable
{
    public class Program
    {
        public Program()
        {
            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddConfiguration(loggingConfiguration.GetSection("Logging"))
                        .AddConsole();
                })
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IDefaultJsonSerializer, DefaultJsonSerializer>()
                .AddScoped<IImageExtractor, ItkImageExtractor>()
                .AddScoped<IItkImageReader, ItkImageReader>()
                .AddScoped<IGifImageWriter, AnimatedGifImageWriter>()
                .AddScoped<IDefaultJsonWriter, DefaultJsonWriter>()
                .AddScoped<IFileSystemStrategy, FileSystemStrategy>()
                .AddSingleton<IAppInfoFactory, AppInfoFactory>()
                .AddSingleton<IAmiConfigurationManager, AmiConfigurationManager>()
                .BuildServiceProvider();

            Logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            ImageService = serviceProvider.GetService<IImageService>();
            Configuration = serviceProvider.GetService<IAmiConfigurationManager>();
        }

        public ILogger Logger { get; }

        public IImageService ImageService { get; }

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
            Logger.LogInformation("AMI.Portable started");

            var input = new ExtractInput();

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       input.DesiredSize = o.DesiredSize;
                       input.AmountPerAxis = o.AmountPerAxis;
                       input.SourcePath = FileSystemHelper.BuildAbsolutePath(o.SourcePath);
                       input.DestinationPath = FileSystemHelper.BuildAbsolutePath(o.DestinationPath);
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
                Process.Start(Path.Combine(input.DestinationPath, output.CombinedGif));
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"AMI.Portable ended after {t.ToReadableTime()}");
        }

        public async Task ExecuteTest(string[] args, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();
            Logger.LogInformation("AMI.Portable started");

            var input = new ExtractInput()
            {
                AmountPerAxis = 10,
                DesiredSize = 250,
                SourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath("animated-medical-imaging"), "data", "SMIR.Brain.XX.O.MR_Flair.36620.mha"),
                DestinationPath = Path.Combine(FileSystemHelper.BuildCurrentPath("animated-medical-imaging"), "temp", Guid.NewGuid().ToString("N")),
                // WatermarkSourcePath = Path.Combine(FileSystemHelper.BuildCurrentPath("animated-medical-imaging"), "data", "watermark.png"),
                OpenCombinedGif = true
            };

            input.AxisTypes.Add(AxisType.Z);

            var output = await ImageService.ExtractAsync(input, ct);

            if (Convert.ToBoolean(input.OpenCombinedGif))
            {
                Process.Start(Path.Combine(input.DestinationPath, output.CombinedGif));
            }

            watch.Stop();

            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);

            Logger.LogInformation($"AMI.Portable ended after {t.ToReadableTime()}");
        }
    }
}
