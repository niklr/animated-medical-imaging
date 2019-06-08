using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Paths.Commands.Process;
using AMI.Domain.Exceptions;

namespace AMI.Core.Extractors
{
    /// <summary>
    /// An extractor for imaging purposes.
    /// </summary>
    public interface IImageExtractor
    {
        /// <summary>
        /// Processes images asynchronous.
        /// </summary>
        /// <param name="command">The command information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The result of the image processing.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// command
        /// or
        /// ct
        /// </exception>
        /// <exception cref="UnexpectedNullException">
        /// Image file extension could not be determined.
        /// or
        /// Filesystem could not be created based on the destination path.
        /// or
        /// Image reader could not be created.
        /// or
        /// Watermark could not be read.
        /// or
        /// Bitmap could not be centered.
        /// </exception>
        Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct);
    }
}
