using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Core.Strategies;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service to upload objects.
    /// </summary>
    /// <seealso cref="IUploadObjectService" />
    public class UploadObjectService : IUploadObjectService
    {
        private readonly string baseUploadPath;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadObjectService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <exception cref="ArgumentNullException">
        /// configuration
        /// or
        /// fileSystemStrategy
        /// </exception>
        public UploadObjectService(IAmiConfigurationManager configuration, IFileSystemStrategy fileSystemStrategy)
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
            string finalFilePath = fileSystem.Path.Combine(localPath, string.Concat(uid, ".ami"));

            try
            {
                int chunkCount = fileSystem.Directory.GetFiles(localPath).Length;
                using (Stream outputStream = File.Create(finalFilePath))
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

                // TODO: move file to binary folder
                var result = new ObjectModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    OriginalFilename = filename
                };

                return await Task.FromResult(result);
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
