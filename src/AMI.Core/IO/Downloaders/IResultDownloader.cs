using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.IO.Downloaders
{
    /// <summary>
    /// A downloader for results.
    /// </summary>
    public interface IResultDownloader
    {
        /// <summary>
        /// Saves the result to the specified stream asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the result.</param>
        /// <param name="stream">The stream to save the compressed result to.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SaveAsync(string id, Stream stream, CancellationToken ct);
    }
}
