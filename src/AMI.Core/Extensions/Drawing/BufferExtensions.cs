using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AMI.Core.Extensions.Drawing
{
    /// <summary>
    /// Extensions related to buffers.
    /// </summary>
    public static class BufferExtensions
    {
        /// <summary>
        /// Return image as bitmap.
        /// </summary>
        /// <param name="intPtr">The pointer to the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The image as bitmap.</returns>
        public static Bitmap ToBitmap(this IntPtr intPtr, int width, int height)
        {
            Bitmap bitmap;

            // Set pixel format as 8-bit grayscale
            // TODO: find mapping between itk.simple.PixelIDValueEnum and System.Drawing.Imaging.PixelFormat
            // see https://github.com/SimpleITK/SimpleITK/issues/582
            PixelFormat format = PixelFormat.Format8bppIndexed;

            // Check if the stride is the same as the width
            if (width % 4 == 0)
            {
                // Width = Stride: simply use the Bitmap constructor
                bitmap = new Bitmap(
                    width,   // Width
                    height,  // Height
                    width,   // Stride
                    format,  // PixelFormat
                    intPtr); // Buffer
            }
            else
            {
                unsafe
                {
                    // Width != Stride: copy data from buffer to bitmap
                    byte* buffer = (byte*)intPtr.ToPointer();

                    // Compute the stride
                    int stride = width;
                    if (width % 4 != 0)
                    {
                        stride = (width / 4 * 4) + 4;
                    }

                    bitmap = new Bitmap(width, height, format);
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, format);

                    // row iteration
                    for (int j = 0; j < height; j++)
                    {
                        byte* row = (byte*)bitmapData.Scan0 + (j * stride);

                        // column iteration
                        for (int i = 0; i < width; i++)
                        {
                            row[i] = buffer[(j * width) + i];
                        }
                    }

                    bitmap.UnlockBits(bitmapData);
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Return image as buffer.
        /// </summary>
        /// <param name="bitmap">The image to be converted.</param>
        /// <returns>The image as buffer.</returns>
        public static Tuple<IntPtr, int> ToBufferAndStride(this Bitmap bitmap)
        {
            BitmapData bitmapData = null;

            try
            {
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                return new Tuple<IntPtr, int>(bitmapData.Scan0, bitmapData.Stride);
            }
            finally
            {
                if (bitmapData != null)
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }
        }
    }
}
