using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Exceptions;
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
        /// <returns>
        /// The output of the image extraction.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// input
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
        Task<ImageExtractOutput> ExtractAsync(ExtractInput input, CancellationToken ct);
    }
}
