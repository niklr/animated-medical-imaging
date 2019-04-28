using System;
using System.Drawing;
using System.Drawing.Imaging;
using AMI.Core.Extensions.Drawing;

namespace AMI.Core.Models
{
    /// <summary>
    /// A container for bitmaps.
    /// </summary>
    public class BitmapContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapContainer"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <exception cref="ArgumentNullException">bitmap</exception>
        public BitmapContainer(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            Format = bitmap.PixelFormat;
            Width = bitmap.Width;
            Height = bitmap.Height;

            var bufferAndStride = bitmap.ToBufferAndStride();
            Buffer = bufferAndStride.Item1;
            Stride = bufferAndStride.Item2;
        }

        /// <summary>
        /// Gets the image buffer.
        /// </summary>
        public IntPtr Buffer { get; }

        /// <summary>
        /// Gets the pixel format.
        /// </summary>
        public PixelFormat Format { get; }

        /// <summary>
        /// Gets the image width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the image height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets or sets the image stride.
        /// </summary>
        public int Stride { get; set; }

        /// <summary>
        /// Converts the buffer to a bitmap.
        /// </summary>
        /// <returns>The buffer as bitmap.</returns>
        public Bitmap ToBitmap()
        {
            return new Bitmap(Width, Height, Stride, Format, Buffer);
        }
    }
}
