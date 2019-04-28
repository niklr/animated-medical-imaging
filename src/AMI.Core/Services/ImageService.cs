using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Extractors;
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
        private readonly IDefaultJsonWriter jsonWriter;
        private readonly IImageExtractor imageExtractor;
        private readonly IGifImageWriter gifImageWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
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

            logger.LogInformation("ImageService created");
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
            logger.LogInformation("Execute started");

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

            if (input.DesiredSize.HasValue)
            {
                imageExtractor.SetDesiredSize(input.DesiredSize.Value);
            }

            if (input.AxisTypes.Count > 0)
            {
                imageExtractor.SetAxisTypes(input.AxisTypes.ToArray());
            }

            imageExtractor.SetImageFormat(input.ImageFormat);
            imageExtractor.SetGrayscale(input.Grayscale);
            imageExtractor.SetWatermarkPath(input.WatermarkSourcePath);

            var destinationFileSystem = fileSystemStrategy.Create(input.DestinationPath);
            destinationFileSystem.Directory.CreateDirectory(input.DestinationPath);

            // TODO: support zip files
            var imageOutput = await imageExtractor.ExtractAsync(input.SourcePath, input.DestinationPath, input.AmountPerAxis, ct);

            var gifs = await gifImageWriter.WriteAsync(input.DestinationPath, imageOutput.Images, input.BezierEasingTypePerAxis, ct);

            var combinedGifFilename = await gifImageWriter.WriteAsync(input.DestinationPath, imageOutput.Images, "combined", input.BezierEasingTypeCombined, ct);

            var output = new ExtractOutput()
            {
                LabelCount = Convert.ToInt32(imageOutput.LabelCount),
                Images = imageOutput.Images,
                Gifs = gifs,
                CombinedGif = combinedGifFilename
            };

            await jsonWriter.WriteAsync(input.DestinationPath, "output", output, (filename) => { output.JsonFilename = filename; });

            return output;
        }
    }
}
