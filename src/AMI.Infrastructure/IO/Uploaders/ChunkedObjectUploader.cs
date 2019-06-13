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

namespace AMI.Infrastructure.IO.Uploaders
{
    /// <summary>
    /// A chunked uploader for objects.
    /// </summary>
    /// <seealso cref="IChunkedObjectUploader" />
    public class ChunkedObjectUploader : IChunkedObjectUploader
    {
        private readonly string baseUploadPath;
        private readonly IFileSystem fileSystem;
        private readonly IApplicationConstants constants;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkedObjectUploader" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="mediator">The mediator.</param>
        /// <exception cref="ArgumentNullException">
        /// configuration
        /// or
        /// fileSystemStrategy
        /// or
        /// objectService
        /// </exception>
        public ChunkedObjectUploader(
            IAmiConfigurationManager configuration,
            IFileSystemStrategy fileSystemStrategy,
            IApplicationConstants constants,
            IMediator mediator)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            fileSystem = fileSystemStrategy.Create(configuration.WorkingDirectory);
            baseUploadPath = fileSystem.Path.Combine(configuration.WorkingDirectory, "Upload", "Objects");

            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        public async Task<UploadChunkResultModel> UploadAsync(int totalChunks, int chunkNumber, string uid, Stream input, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                throw new ArgumentNullException(nameof(uid));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            string localPath = CreateLocalUploadPath(uid);
            string chunkFilename = GetChunkedFilename(chunkNumber, uid);
            string chunkFilePath = fileSystem.Path.Combine(localPath, chunkFilename);

            // TODO: define max chunk count
            int currentChunkCount = fileSystem.Directory.GetFiles(localPath).Length;

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
                        string currentChunkFilename = GetChunkedFilename(chunkNumber, uid);
                        string currentChunkFilePath = fileSystem.Path.Combine(localPath, currentChunkFilename);

                        if (File.Exists(currentChunkFilePath))
                        {
                            Stream inputStream = File.OpenRead(currentChunkFilePath);

                            int len;
                            while ((len = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputStream.Write(buffer, 0, len);
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
