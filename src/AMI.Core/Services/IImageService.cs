using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Models;

namespace AMI.Core.Services
{
    /// <summary>
    /// A service for imaging purposes.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Extracts images based on the provided input information asynchronous.
        /// </summary>
        /// <param name="input">The input information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The output information.</returns>
        Task<ExtractOutput> ExtractAsync(ExtractInput input, CancellationToken ct);
    }
}
