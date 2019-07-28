using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;

namespace AMI.Core.IO.Extractors
{
    /// <summary>
    /// An extractor for imaging purposes.
    /// </summary>
    public interface IImageExtractor
    {
        /// <summary>
        /// Processes the images asynchronous.
        /// </summary>
        /// <param name="command">The command information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The result of the image processing.
        /// </returns>
        Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct);
    }
}
