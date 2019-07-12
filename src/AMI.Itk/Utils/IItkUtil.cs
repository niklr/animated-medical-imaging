using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Enums;
using itk.simple;

namespace AMI.Itk.Utils
{
    /// <summary>
    /// An utility based on the Insight Segmentation and Registration Toolkit (ITK).
    /// </summary>
    internal interface IItkUtil
    {
        /// <summary>
        /// Reads the image asynchronous.
        /// </summary>
        /// <param name="path">The location of the image on the file system.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// path
        /// or
        /// ct
        /// </exception>
        Task<Image> ReadImageAsync(string path, CancellationToken ct);

        /// <summary>
        /// Writes the image asynchronous.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <param name="path">The file system location where the image should be written.</param>
        /// <param name="filename">The name of the file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// image
        /// or
        /// path
        /// or
        /// filename
        /// or
        /// ct
        /// </exception>
        Task WriteImageAsync(Image image, string path, string filename, CancellationToken ct);

        /// <summary>
        /// Extracts the position on the specified axis.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="index">The position.</param>
        /// <returns>
        /// The extracted position as two-dimensional ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        Image ExtractPosition(Image image, AxisType axisType, uint index);

        /// <summary>
        /// Resamples the two-dimensional ITK image to the desired size.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image.</param>
        /// <param name="outputSize">The output size.</param>
        /// <returns>
        /// The resampled two-dimensional ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        /// <exception cref="NotSupportedException">The dimension ({dimension}) of the provided image is not supported.</exception>
        Image ResampleImage2D(Image image, uint outputSize);

        /// <summary>
        /// Gets the number of labels in the ITK image.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <returns>The number of labels in the ITK image.</returns>
        /// <exception cref="ArgumentNullException">image</exception>
        ulong GetLabelCount(Image image);

        /// <summary>
        /// Converts the two-dimensional ITK image to a bitmap.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image</param>
        /// <returns>
        /// The image as bitmap.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        /// <exception cref="NotSupportedException">The dimension ({dimension}) of the provided image is not supported.</exception>
        System.Drawing.Bitmap ToBitmap(Image image);
    }
}
