using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Objects.Commands.Create;
using AMI.Core.IO.Uploaders;
using AMI.Core.Strategies;
using MediatR;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.IO.Uploaders
{
    /// <summary>
    /// A chunked uploader for objects.
    /// </summary>
    /// <seealso cref="IChunkedObjectUploader" />
    public class ChunkedObjectUploader : IChunkedObjectUploader
    {
        private readonly IMediator mediator;
        private readonly IApplicationConstants constants;
        private readonly IAppConfiguration configuration;
        private readonly string baseUploadPath;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkedObjectUploader" /> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public ChunkedObjectUploader(
            IMediator mediator,
            IApplicationConstants constants,
            IAppConfiguration configuration,
            IFileSystemStrategy fileSystemStrategy)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Ensure.ArgumentNotNull(fileSystemStrategy, nameof(fileSystemStrategy));

            fileSystem = fileSystemStrategy.Create(configuration.Options.WorkingDirectory);
            baseUploadPath = fileSystem.Path.Combine(configuration.Options.WorkingDirectory, "Upload", "Objects");
        }

        /// <inheritdoc/>
        public async Task<UploadChunkResultModel> UploadAsync(int totalChunks, int chunkNumber, string uid, Stream input, CancellationToken ct)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(uid, nameof(uid));
            Ensure.ArgumentNotNull(input, nameof(input));

            if (input.Length < constants.MinUploadChunkSize || input.Length > constants.MaxUploadChunkSize)
            {
                throw new ArgumentException($"The size of a single chunk must be between {constants.MinUploadChunkSize} and {constants.MaxUploadChunkSize} bytes.");
            }

            string localPath = CreateLocalUploadPath(uid);
            string chunkFilename = GetChunkedFilename(chunkNumber, uid);
            string chunkFilePath = fileSystem.Path.Combine(localPath, chunkFilename);

            int currentChunkCount = fileSystem.Directory.GetFiles(localPath).Length;

            if (configuration.Options.MaxSizeKilobytes > 0 &&
                currentChunkCount * input.Length > configuration.Options.MaxSizeKilobytes * 1000)
            {
                fileSystem.Directory.Delete(localPath, true);
                throw new ArgumentException($"The file size exceeds the limit of {configuration.Options.MaxSizeKilobytes} kilobytes.");
            }

            using (Stream output = fileSystem.File.OpenWrite(chunkFilePath))
            {
                input.CopyTo(output);
            }

            var result = new UploadChunkResultModel()
            {
                IsCompleted = currentChunkCount + 1 == totalChunks
            };

            return await Task.FromResult(result);
        }

        /// <inheritdoc/>
        public async Task<ObjectModel> CommitAsync(string filename, string fullDestPath, string uid, CancellationToken ct)
        {
            string localPath = CreateLocalUploadPath(uid);
            string destFilename = string.Concat(uid, constants.DefaultFileExtension);
            string destFilePath = fileSystem.Path.Combine(localPath, destFilename);

            try
            {
                int chunkCount = fileSystem.Directory.GetFiles(localPath).Length;
                using (Stream outputStream = File.Create(destFilePath))
                {
                    byte[] buffer = new byte[8 * 1024];

                    for (int chunkNumber = 1; chunkNumber <= chunkCount; chunkNumber++)
                    {
                        ct.ThrowIfCancellationRequested();

                        string currentChunkFilename = GetChunkedFilename(chunkNumber, uid);
                        string currentChunkFilePath = fileSystem.Path.Combine(localPath, currentChunkFilename);

                        if (File.Exists(currentChunkFilePath))
                        {
                            Stream inputStream = File.OpenRead(currentChunkFilePath);

                            int len;
                            while ((len = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputStream.Write(buffer, 0, len);

                                if (configuration.Options.MaxSizeKilobytes > 0 &&
                                    outputStream.Length > configuration.Options.MaxSizeKilobytes * 1000)
                                {
                                    throw new ArgumentException($"The file size exceeds the limit of {configuration.Options.MaxSizeKilobytes} kilobytes.");
                                }
                            }

                            inputStream.Close();
                        }
                        else
                        {
                            throw new ArgumentException(string.Format("Chunk number {0} is missing.", chunkNumber));
                        }
                    }
                }

                var command = new CreateObjectCommand()
                {
                    OriginalFilename = filename,
                    SourcePath = destFilePath
                };

                return await mediator.Send(command, ct);
            }
            finally
            {
                fileSystem.Directory.Delete(localPath, true);
            }
        }

        /// <summary>
        /// Creates the local upload path.
        /// </summary>
        /// <param name="uid">The unique identifier.</param>
        /// <returns>The local upload path based on the provided uid.</returns>
        /// <exception cref="ArgumentNullException">uid</exception>
        /// <exception cref="Exception">Base upload path is not defined.</exception>
        private string CreateLocalUploadPath(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }

            string path = fileSystem.Path.Combine(baseUploadPath, uid);
            fileSystem.Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>
        /// Gets the chunked filename.
        /// </summary>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <returns>The chunked filename.</returns>
        private string GetChunkedFilename(int chunkNumber, string uid)
        {
            return string.Concat(uid, chunkNumber > 0 ? $".part{chunkNumber}" : string.Empty);
        }
    }
}
