using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Process;
using AMI.Core.Extensions.FileSystemExtensions;
using AMI.Core.Extractors;
using AMI.Core.Factories;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.ObjectExtensions;

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
        /// Processes images based on the provided command information asynchronous.
        /// </summary>
        /// <param name="command">The command information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The result information.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// command
        /// or
        /// ct
        /// </exception>
        /// <exception cref="UnexpectedNullException">
        /// Empty source path.
        /// or
        /// Empty destination path.
        /// </exception>
        public async Task<ProcessResult> ProcessAsync(ProcessObjectCommand command, CancellationToken ct)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            if (string.IsNullOrWhiteSpace(command.SourcePath))
            {
                throw new UnexpectedNullException("Empty source path.");
            }

            if (string.IsNullOrWhiteSpace(command.DestinationPath))
            {
                throw new UnexpectedNullException("Empty destination path.");
            }

            var sourceFs = fileSystemStrategy.Create(command.SourcePath);
            var destFs = fileSystemStrategy.Create(command.DestinationPath);
            var watermarkFs = fileSystemStrategy.Create(command.WatermarkSourcePath);

            var commandClone = command.DeepClone();

            commandClone.SourcePath = sourceFs.BuildAbsolutePath(command.SourcePath);
            commandClone.DestinationPath = destFs.BuildAbsolutePath(command.DestinationPath);
            commandClone.WatermarkSourcePath = watermarkFs.BuildAbsolutePath(command.WatermarkSourcePath);

            // Creates all directories and subdirectories in the specified path unless they already exist.
            destFs.Directory.CreateDirectory(commandClone.DestinationPath);

            // TODO: support zip files
            var imageResult = await imageExtractor.ProcessAsync(commandClone, ct);
            var gifs = await gifImageWriter.WriteAsync(commandClone.DestinationPath, imageResult.Images, commandClone.BezierEasingTypePerAxis, ct);
            var combinedGifFilename = await gifImageWriter.WriteAsync(commandClone.DestinationPath, imageResult.Images, "combined", commandClone.BezierEasingTypeCombined, ct);

            var appInfo = appInfoFactory.Create();
            var result = new ProcessResult()
            {
                Version = appInfo.AppVersion,
                LabelCount = Convert.ToInt32(imageResult.LabelCount),
                Images = imageResult.Images,
                Gifs = gifs,
                CombinedGif = combinedGifFilename
            };

            await jsonWriter.WriteAsync(commandClone.DestinationPath, "output", result, (filename) => { result.JsonFilename = filename; });

            return result;
        }
    }
}
