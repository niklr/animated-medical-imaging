using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Models;

namespace AMI.Core.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    public interface ICompressibleExtractor
    {
        /// <summary>
        /// Extracts the compressed file asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of compressed entries.</returns>
        Task<IList<CompressedEntry>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct);
    }
}
