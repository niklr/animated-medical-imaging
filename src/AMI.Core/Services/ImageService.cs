using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Extensions.FileSystemExtensions;
using AMI.Core.Extensions.ObjectExtensions;
using AMI.Core.Extractors;
using AMI.Core.Factories;
using AMI.Core.Models;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service for imaging purposes.
    /// </summary>
    /// <seealso cref="IImageService" />
    public class ImageService : IImageService
    {
        private readonly ILogger logger;
        private readonly IAmiConfigurationManager configuration;
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IAppInfoFactory appInfoFactory;
        private readonly IDefaultJsonWriter jsonWriter;
        private readonly IImageExtractor imageExtractor;
        private readonly IGifImageWriter gifImageWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="appInfoFactory">The application information factory.</param>
        /// <param name="jsonWriter">The JSON writer.</param>
        /// <param name="imageExtractor">The image extractor.</param>
        /// <param name="gifImageWriter">The GIF image writer.</param>
        /// <exception cref="ArgumentNullException">
        /// loggerFactory
        /// or
        /// configuration
        /// or
        /// fileSystemStrategy
        /// or
        /// appInfoFactory
        /// or
        /// jsonWriter
        /// or
        /// imageExtractor
        /// or
        /// gifImageWriter
        /// </exception>
        public ImageService(
            ILoggerFactory loggerFactory,
            IAmiConfigurationManager configuration,
            IFileSystemStrategy fileSystemStrategy,
            IAppInfoFactory appInfoFactory,
            IDefaultJsonWriter jsonWriter,
            IImageExtractor imageExtractor,
            IGifImageWriter gifImageWriter)
        {
            logger = loggerFactory?.CreateLogger<ImageService>();
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.configuration = configuration;
            if (this.configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.fileSystemStrategy = fileSystemStrategy;
            if (this.fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            this.appInfoFactory = appInfoFactory;
            if (this.appInfoFactory == null)
            {
                throw new ArgumentNullException(nameof(appInfoFactory));
            }

            this.jsonWriter = jsonWriter;
            if (this.jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            this.imageExtractor = imageExtractor;
            if (this.imageExtractor == null)
            {
                throw new ArgumentNullException(nameof(imageExtractor));
            }

            this.gifImageWriter = gifImageWriter;
            if (this.gifImageWriter == null)
            {
                throw new ArgumentNullException(nameof(gifImageWriter));
            }
        }

        /// <summary>
        /// Extracts images based on the provided input information asynchronous.
        /// </summary>
        /// <param name="input">The input information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The output information.
        /// </returns>
        /// <exception cref="ArgumentNullException">input</exception>
        /// <exception cref="ArgumentException">
        /// Empty source path.
        /// or
        /// Empty destination path.
        /// </exception>
        public async Task<ExtractOutput> ExtractAsync(ExtractInput input, CancellationToken ct)
        {
            logger.LogInformation("ImageService ExtractAsync started");

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input.SourcePath))
            {
                throw new ArgumentException("Empty source path.");
            }

            if (string.IsNullOrWhiteSpace(input.DestinationPath))
            {
                throw new ArgumentException("Empty destination path.");
            }

            var sourceFs = fileSystemStrategy.Create(input.SourcePath);
            var destFs = fileSystemStrategy.Create(input.DestinationPath);
            var watermarkFs = fileSystemStrategy.Create(input.WatermarkSourcePath);

            var inputClone = input.DeepClone();

            inputClone.SourcePath = sourceFs.BuildAbsolutePath(input.SourcePath);
            inputClone.DestinationPath = destFs.BuildAbsolutePath(input.DestinationPath);
            inputClone.WatermarkSourcePath = watermarkFs.BuildAbsolutePath(input.WatermarkSourcePath);

            // Creates all directories and subdirectories in the specified path unless they already exist.
            destFs.Directory.CreateDirectory(inputClone.DestinationPath);

            // TODO: support zip files
            var imageOutput = await imageExtractor.ExtractAsync(inputClone, ct);
            var gifs = await gifImageWriter.WriteAsync(inputClone.DestinationPath, imageOutput.Images, inputClone.BezierEasingTypePerAxis, ct);
            var combinedGifFilename = await gifImageWriter.WriteAsync(inputClone.DestinationPath, imageOutput.Images, "combined", inputClone.BezierEasingTypeCombined, ct);

            var appInfo = appInfoFactory.Create();
            var output = new ExtractOutput()
            {
                Version = appInfo.AppVersion,
                LabelCount = Convert.ToInt32(imageOutput.LabelCount),
                Images = imageOutput.Images,
                Gifs = gifs,
                CombinedGif = combinedGifFilename
            };

            await jsonWriter.WriteAsync(inputClone.DestinationPath, "output", output, (filename) => { output.JsonFilename = filename; });

            logger.LogInformation("ImageService ExtractAsync ended");

            return output;
        }
    }
}
