using System;
using System.Drawing.Imaging;
using System.Linq;

namespace AMI.Core.Extensions.Drawing
{
    /// <summary>
    /// Extensions related to image formats.
    /// </summary>
    public static class ImageFormatExtensions
    {
        /// <summary>
        /// Gets the extension of the file based on the image format.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns>The extension of the file.</returns>
        public static string FileExtensionFromEncoder(this ImageFormat format)
        {
            try
            {
                return ImageCodecInfo.GetImageEncoders()
                        .First(x => x.FormatID == format.Guid)
                        .FilenameExtension
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .First()
                        .Trim('*')
                        .ToLower();
            }
            catch (Exception)
            {
                return ".IDFK";
            }
        }
    }
}
