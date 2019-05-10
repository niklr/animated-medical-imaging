using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.Uploaders
{
    /// <summary>
    /// This interfaces represents a resumable uploader.
    /// </summary>
    public interface IResumableUploader
    {
        /// <summary>
        /// Uploads the specified chunk number.
        /// </summary>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="input">The input stream.</param>
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
        Task CommitAsync(string filename, string fullDestPath, string uid, CancellationToken ct = default);
    }
}
