using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;

namespace AMI.Core.Writers
{
    /// <summary>
    /// A writer for GIF images.
    /// </summary>
    public interface IGifImageWriter
    {
        /// <summary>
        /// Writes the GIF images asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="images">The images.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The output information for each axis.
        /// </returns>
        /// <exception cref="AmiException">
        /// The writing of the GIF has been cancelled.
        /// or
        /// The GIF could not be written.
        /// </exception>
        Task<IReadOnlyList<AxisContainer<string>>> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainer<string>> images,
            BezierEasingType bezierEasingType,
            CancellationToken ct);

        /// <summary>
        /// Writes the GIF image asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="images">The images.</param>
        /// <param name="name">The name of the GIF image without file extension.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The filename of the GIF image.
        /// </returns>
        /// <exception cref="AmiException">
        /// The writing of the GIF has been cancelled.
        /// or
        /// The GIF could not be written.
        /// </exception>
        Task<string> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainer<string>> images,
            string name,
            BezierEasingType bezierEasingType,
            CancellationToken ct);
    }
}
