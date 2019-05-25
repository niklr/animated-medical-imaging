using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AMI.Core.Extensions.Drawing
{
    /// <summary>
    /// Extensions related to images.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts the image to a byte array.
        /// </summary>
        /// <param name="image">The image to be converted.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>
        /// The image as byte array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// image
        /// or
        /// imageFormat
        /// </exception>
        public static byte[] ToByteArray(this Image image, ImageFormat imageFormat)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (imageFormat == null)
            {
                throw new ArgumentNullException(nameof(imageFormat));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }
    }
}
