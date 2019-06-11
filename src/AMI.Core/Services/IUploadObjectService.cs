using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service to upload objects.
    /// </summary>
    public interface IUploadObjectService
    {
        /// <summary>
        /// Uploads the specified chunk number.
        /// </summary>
        /// <param name="totalChunks">The total chunks.</param>
        /// <param name="chunkNumber">The chunk number.</param>
        /// <param name="uid">The unique identifier.</param>
        /// <param name="input">The input stream.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <exception cref="ArgumentNullException">
        /// uid
        /// or
        /// input
        /// </exception>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task<UploadChunkResultModel> UploadAsync(int totalChunks, int chunkNumber, string uid, Stream input, CancellationToken ct);

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
        Task<ObjectModel> CommitAsync(string filename, string fullDestPath, string uid, CancellationToken ct);
    }
}
