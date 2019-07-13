using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Enums;
using itk.simple;

[assembly: InternalsVisibleTo("AMI.NetCore.Tests")]
[assembly: InternalsVisibleTo("AMI.NetFramework.Tests")]

namespace AMI.Itk.Utils
{
    /// <summary>
    /// An utility based on the Insight Segmentation and Registration Toolkit (ITK).
    /// </summary>
    internal interface IItkUtil
    {
        /// <summary>
        /// Creates the image reader depending on the provided path being a directory or file.
        /// </summary>
        /// <param name="path">The location of the directory or file on the file system.</param>
        /// <returns>
        /// The image reader based on the provided path.
        /// </returns>
        ImageReaderBase CreateImageReader(string path);

        /// <summary>
        /// Discovers the entry path based on the provided file names.
        /// </summary>
        /// <param name="files">The names of files (including their paths) in a directory.</param>
        /// <returns>The entry path.</returns>
        string DiscoverEntryPath(string[] files);

        /// <summary>
        /// Reads the image asynchronous.
        /// </summary>
        /// <param name="path">The location of the image on the file system.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The ITK image.
        /// </returns>
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
        Image ExtractPosition(Image image, AxisType axisType, uint index);

        /// <summary>
        /// Resamples the two-dimensional ITK image to the desired size.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image.</param>
        /// <param name="outputSize">The output size.</param>
        /// <returns>
        /// The resampled two-dimensional ITK image.
        /// </returns>
        Image ResampleImage2D(Image image, uint outputSize);

        /// <summary>
        /// Resamples the three-dimensional ITK image to the desired size.
        /// </summary>
        /// <param name="image">The three-dimensional ITK image.</param>
        /// <param name="outputSize">The output size.</param>
        /// <returns>
        /// The resampled three-dimensional ITK image.
        /// </returns>
        Image ResampleImage3D(Image image, uint outputSize);

        /// <summary>
        /// Gets the number of labels in the ITK image.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <returns>
        /// The number of labels in the ITK image.
        /// </returns>
        ulong GetLabelCount(Image image);

        /// <summary>
        /// Converts the two-dimensional ITK image to a bitmap.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image</param>
        /// <returns>
        /// The image as bitmap.
        /// </returns>
        System.Drawing.Bitmap ToBitmap(Image image);
    }
}
