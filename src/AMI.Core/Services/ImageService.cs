using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Process;
using AMI.Core.Entities.Paths.Commands.Process;
using AMI.Core.Entities.Shared.Commands;
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

        /// <inheritdoc/>
        public async Task<ProcessResultModel> ProcessAsync(BaseProcessCommand command, CancellationToken ct)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            switch (command)
            {
                case ProcessPathCommand pathCommand:
                    return await ProcessPathAsync(pathCommand, ct);
                case ProcessObjectCommand objectCommand:
                    return await ProcessObjectAsync(objectCommand, ct);
                default:
                    throw new NotSupportedException("Process command type is not supported.");
            }
        }

        private async Task<ProcessResultModel> ProcessPathAsync(ProcessPathCommand command, CancellationToken ct)
        {
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
            var result = await imageExtractor.ProcessAsync(commandClone, ct);

            // Set GIFs
            result.Gifs = await gifImageWriter.WriteAsync(
                command.DestinationPath, result.Images, command.BezierEasingTypePerAxis, ct);
            result.CombinedGif = await gifImageWriter.WriteAsync(
                command.DestinationPath, result.Images, "combined", command.BezierEasingTypeCombined, ct);

            // Set application version
            var appInfo = appInfoFactory.Create();
            result.Version = appInfo.AppVersion;

            // Write JSON and set filename
            await jsonWriter.WriteAsync(commandClone.DestinationPath, "output", result, (filename) => { result.JsonFilename = filename; });

            return result;
        }

        private async Task<ProcessResultModel> ProcessObjectAsync(ProcessObjectCommand command, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
            {
                throw new UnexpectedNullException("Empty identifier.");
            }

            return null;
        }
    }
}
