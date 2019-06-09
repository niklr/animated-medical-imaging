using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service for resumable uploads.
    /// </summary>
    public interface IResumableUploadService
    {
        /// <summary>
        /// Uploads the specified chunk number.
        /// </summary>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="input">The input stream.</param>
        /// <exception cref="ArgumentNullException">
        /// uid
        /// or
        /// input
        /// </exception>
        void Upload(int chunkNumber, string uid, Stream input);

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
        Task CommitAsync(string filename, string fullDestPath, string uid, CancellationToken ct);
    }
}
