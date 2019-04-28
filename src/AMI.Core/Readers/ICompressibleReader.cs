using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Models;

namespace AMI.Core.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public interface ICompressibleReader
    {
        /// <summary>
        /// Reads the specified compressed file asynchronous.
        /// </summary>
        /// <param name="path">The location of the compressed file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of compressed entries.</returns>
        Task<IList<CompressedEntry>> ReadAsync(string path, CancellationToken ct);
    }
}
