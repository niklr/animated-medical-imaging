using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Strategies;

namespace AMI.Core.Uploaders
{
    /// <summary>
    /// A resumable uploader.
    /// </summary>
    /// <seealso cref="IResumableUploader" />
    public class ResumableUploader : IResumableUploader
    {
        private readonly string baseUploadPath;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResumableUploader" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <exception cref="ArgumentNullException">
        /// baseUploadPath
        /// or
        /// fileSystemStrategy
        /// </exception>
        public ResumableUploader(IAmiConfigurationManager configuration, IFileSystemStrategy fileSystemStrategy)
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
            baseUploadPath = fileSystem.Path.Combine(configuration.WorkingDirectory, "ResumableUpload");
        }

        /// <summary>
        /// Uploads the specified chunk number.
        /// </summary>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="input">The input stream.</param>
        public void Upload(int chunkNumber, string uid, Stream input)
        {
            string localPath = CreateLocalUploadPath(uid);
            string filename = string.Concat(uid, chunkNumber > 0 ? $".part{chunkNumber}" : string.Empty);
            string outputFile = fileSystem.Path.Combine(localPath, filename);

            using (Stream output = fileSystem.File.OpenWrite(outputFile))
            {
                input.CopyTo(output);
            }
        }

        /// <summary>
        /// Commits the file to the desired storage location.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="fullDestPath">The full destination path including the filename.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public Task CommitAsync(string filename, string fullDestPath, string uid, CancellationToken ct)
        {
            throw new NotImplementedException();
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
    }
}
