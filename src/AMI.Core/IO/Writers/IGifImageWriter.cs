using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums;

namespace AMI.Core.IO.Writers
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
        /// <param name="delay">The delay between frames.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The output information for each axis.
        /// </returns>
        Task<IReadOnlyList<AxisContainerModel<string>>> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainerModel<string>> images,
            int delay,
            BezierEasingType bezierEasingType,
            CancellationToken ct);

        /// <summary>
        /// Writes the GIF image asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="images">The images.</param>
        /// <param name="name">The name of the GIF image without file extension.</param>
        /// <param name="delay">The delay between frames.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The filename of the GIF image.
        /// </returns>
        Task<string> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainerModel<string>> images,
            string name,
            int delay,
            BezierEasingType bezierEasingType,
            CancellationToken ct);
    }
}
