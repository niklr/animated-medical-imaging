using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Models;

namespace AMI.Core.Extractors
{
    /// <summary>
    /// An extractor for imaging purposes.
    /// </summary>
    public interface IImageExtractor
    {
        /// <summary>
        /// Extracts images asynchronous.
        /// </summary>
        /// <param name="input">The input information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The output information.</returns>
        Task<ImageExtractOutput> ExtractAsync(ExtractInput input, CancellationToken ct);
    }
}
