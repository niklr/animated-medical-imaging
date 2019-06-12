using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Mappers;
using AMI.Domain.Enums;

namespace AMI.Core.IO.Readers
{
    /// <summary>
    /// A reader for images.
    /// </summary>
    /// <typeparam name="T">The type of the image.</typeparam>
    public interface IImageReader<T>
    {
        /// <summary>
        /// Gets the image.
        /// </summary>
        T Image { get; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        uint Width { get; }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        uint Height { get; }

        /// <summary>
        /// Gets the depth of the image.
        /// </summary>
        uint Depth { get; }

        /// <summary>
        /// Gets or sets the axis positions mapper.
        /// </summary>
        IAxisPositionMapper Mapper { get; set; }

        /// <summary>
        /// Initializes the reader asynchronous.
        /// </summary>
        /// <param name="path">The location of the image.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InitAsync(string path, CancellationToken ct);

        /// <summary>
        /// Gets the recommended axis types.
        /// </summary>
        /// <returns>The recommended axis types.</returns>
        ISet<AxisType> GetRecommendedAxisTypes();

        /// <summary>
        /// Extracts the specified position as bitmap.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The desired output size.</param>
        /// <returns>The extracted position as bitmap.</returns>
        System.Drawing.Bitmap ExtractPosition(AxisType axisType, uint position, uint? size);

        /// <summary>
        /// Gets the label count.
        /// </summary>
        /// <returns>The label count.</returns>
        ulong GetLabelCount();
    }
}
