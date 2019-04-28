using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Enums;
using AMI.Core.Models;

namespace AMI.Core.Extractors
{
    /// <summary>
    /// An extractor for imaging purposes.
    /// </summary>
    public interface IImageExtractor
    {
        /// <summary>
        /// Sets the desired size of the extracted images.
        /// </summary>
        /// <param name="desiredSize">The desired size.</param>
        void SetDesiredSize(uint desiredSize);

        /// <summary>
        /// Sets the axis types.
        /// </summary>
        /// <param name="axisTypes">The axis types.</param>
        void SetAxisTypes(params AxisType[] axisTypes);

        /// <summary>
        /// Sets the image format.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        void SetImageFormat(ImageFormat imageFormat);

        /// <summary>
        /// Sets the value indicating whether the images should be converted to grayscale.
        /// </summary>
        /// <param name="grayscale">if set to <c>true</c> images should be converted to grayscale.</param>
        void SetGrayscale(bool grayscale);

        /// <summary>
        /// Sets the path of the watermark.
        /// </summary>
        /// <param name="path">The path.</param>
        void SetWatermarkPath(string path);

        /// <summary>
        /// Extracts images asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="amount">The amount of images to be extracted.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The output information.</returns>
        Task<ImageExtractOutput> ExtractAsync(string sourcePath, string destinationPath, uint amount, CancellationToken ct);
    }
}
