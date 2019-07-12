using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Extensions.FileSystemExtensions;
using AMI.Core.Factories;
using AMI.Core.IO.Extractors;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Writers;
using AMI.Core.Services;
using AMI.Core.Strategies;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using RNS.Framework.Extensions.ObjectExtensions;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for imaging purposes.
    /// </summary>
    /// <seealso cref="IImageService" />
    public class ImageService : IImageService
    {
        private readonly ILogger logger;
        private readonly IAppConfiguration configuration;
        private readonly IArchiveReader archiveReader;
        private readonly IArchiveExtractor archiveExtractor;
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
        /// <param name="archiveReader">The archive reader.</param>
        /// <param name="archiveExtractor">The archive extractor.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="appInfoFactory">The application information factory.</param>
        /// <param name="jsonWriter">The JSON writer.</param>
        /// <param name="imageExtractor">The image extractor.</param>
        /// <param name="gifImageWriter">The GIF image writer.</param>
        public ImageService(
            ILoggerFactory loggerFactory,
            IAppConfiguration configuration,
            IArchiveReader archiveReader,
            IArchiveExtractor archiveExtractor,
            IFileSystemStrategy fileSystemStrategy,
            IAppInfoFactory appInfoFactory,
            IDefaultJsonWriter jsonWriter,
            IImageExtractor imageExtractor,
            IGifImageWriter gifImageWriter)
        {
            logger = loggerFactory?.CreateLogger<ImageService>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.archiveReader = archiveReader ?? throw new ArgumentNullException(nameof(archiveReader));
            this.archiveExtractor = archiveExtractor ?? throw new ArgumentNullException(nameof(archiveExtractor));
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.appInfoFactory = appInfoFactory ?? throw new ArgumentNullException(nameof(appInfoFactory));
            this.jsonWriter = jsonWriter ?? throw new ArgumentNullException(nameof(jsonWriter));
            this.imageExtractor = imageExtractor ?? throw new ArgumentNullException(nameof(imageExtractor));
            this.gifImageWriter = gifImageWriter ?? throw new ArgumentNullException(nameof(gifImageWriter));
        }

        /// <inheritdoc/>
        public async Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct)
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

            // SourcePath can be a directory, archive or file
            ProcessResultModel result = null;

            if (sourceFs.IsDirectory(commandClone.SourcePath))
            {
                // TODO: process directory
            }
            else if (archiveReader.IsArchive(command.SourcePath))
            {
                var extractedPath = destFs.Path.Combine(command.DestinationPath, "Extracted");
                destFs.Directory.CreateDirectory(extractedPath);
                await archiveExtractor.ExtractAsync(command.SourcePath, extractedPath, ct);

                // TODO: process directory
            }
            else
            {
                result = await imageExtractor.ProcessAsync(commandClone, ct);
            }

            if (result == null)
            {
                throw new UnexpectedNullException("The images could not be processed.");
            }

            // Set GIFs
            result.Gifs = await gifImageWriter.WriteAsync(
                commandClone.DestinationPath, result.Images, commandClone.BezierEasingTypePerAxis, ct);
            result.CombinedGif = await gifImageWriter.WriteAsync(
                commandClone.DestinationPath, result.Images, "combined", commandClone.BezierEasingTypeCombined, ct);

            // Set application version
            var appInfo = appInfoFactory.Create();
            result.Version = appInfo.AppVersion;

            // Write JSON and set filename
            await jsonWriter.WriteAsync(commandClone.DestinationPath, "output", result, (filename) => { result.JsonFilename = filename; });

            return result;
        }
    }
}
