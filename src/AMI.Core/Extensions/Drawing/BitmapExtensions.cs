using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AMI.Core.Wrappers;
using RNS.Framework.Tools;

namespace AMI.Core.Extensions.Drawing
{
    /// <summary>
    /// Extensions related to bitmaps.
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Converts the provided image to grayscale.
        /// Source: https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <returns>
        /// The image in grayscale.
        /// </returns>
        /// <exception cref="ArgumentNullException">original</exception>
        public static Bitmap ToGrayscale(this Bitmap original)
        {
            Ensure.ArgumentNotNull(original, nameof(original));

            // create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            // get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                // create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                    new float[] { .3f, .3f, .3f, 0, 0 },
                    new float[] { .59f, .59f, .59f, 0, 0 },
                    new float[] { .11f, .11f, .11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                   });

                // create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                // set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                // draw the original image on the new image
                // using the grayscale color matrix
                g.DrawImage(
                    original,
                    new Rectangle(0, 0, original.Width, original.Height),
                    0,
                    0,
                    original.Width,
                    original.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return newBitmap;
        }

        /// <summary>
        /// Converts the provided image to grayscale.
        /// The format is 8 bits per pixel, indexed. The color table therefore has 256 colors in it.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <returns>
        /// The image in grayscale.
        /// </returns>
        /// <exception cref="ArgumentNullException">original</exception>
        public static Bitmap To8bppIndexedGrayscale(this Bitmap original)
        {
            Ensure.ArgumentNotNull(original, nameof(original));

            // Set pixel format as 8-bit grayscale
            PixelFormat format = PixelFormat.Format8bppIndexed;

            // Create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            // Get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                // Copy the original palette
                ColorPalette originalPalette = original.Palette;

                // Set the palette before drawing the image
                original.Palette = CreateGrayscalePalette(format, 256);

                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height));

                // Set original palette
                original.Palette = originalPalette;
            }

            return newBitmap;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <param name="size">The desired size.</param>
        /// <returns>
        /// The resized image.
        /// </returns>
        /// <exception cref="ArgumentNullException">original</exception>
        public static Bitmap Resize(this Bitmap original, uint? size)
        {
            Ensure.ArgumentNotNull(original, nameof(original));

            if (size.HasValue && size.Value > 0)
            {
                int desiredSize = Convert.ToInt32(size.Value);
                if (original.Width == desiredSize && original.Height == desiredSize)
                {
                    return original;
                }

                double oldw = original.Width;
                double oldh = original.Height;
                double neww, newh = 0;
                double rw = oldw / desiredSize;
                double rh = oldh / desiredSize;

                if (rw > rh)
                {
                    newh = oldh / rw;
                    neww = desiredSize;
                }
                else
                {
                    neww = oldw / rh;
                    newh = desiredSize;
                }

                var destRect = new Rectangle(0, 0, Convert.ToInt32(neww), Convert.ToInt32(newh));
                var destImage = new Bitmap(destRect.Width, destRect.Height);

                destImage.SetResolution(original.HorizontalResolution, original.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(original, destRect, 0, 0, Convert.ToInt32(oldw), Convert.ToInt32(oldh), GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }

            return original;
        }

        /// <summary>
        /// Centers the provided image.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="size">The size.</param>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The centered image.
        /// </returns>
        /// <exception cref="ArgumentNullException">original</exception>
        public static Bitmap ToCenter(this Bitmap original, uint? size, Color color)
        {
            Ensure.ArgumentNotNull(original, nameof(original));

            if (size.HasValue && size.Value > 0)
            {
                int desiredSize = Convert.ToInt32(size.Value);
                if (original.Width == desiredSize && original.Height == desiredSize)
                {
                    return original;
                }

                Bitmap result = new Bitmap(desiredSize, desiredSize);
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.Clear(color);
                    int x = (result.Width / 2) - (original.Width / 2);
                    int y = (result.Height / 2) - (original.Height / 2);
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.DrawImage(original, x, y, original.Width, original.Height);
                }

                return result;
            }

            return original;
        }

        /// <summary>
        /// Appends the watermark to the image.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <param name="watermark">The watermark.</param>
        /// <returns>
        /// The image containing the watermark.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// original
        /// or
        /// watermark
        /// </exception>
        public static Bitmap AppendWatermark(this Bitmap original, BitmapWrapper watermark)
        {
            Ensure.ArgumentNotNull(original, nameof(original));
            Ensure.ArgumentNotNull(watermark, nameof(watermark));

            var destRect = new Rectangle(0, 0, original.Width, original.Height);
            var destImage = new Bitmap(original.Width, original.Height);

            destImage.SetResolution(original.HorizontalResolution, original.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            using (TextureBrush watermarkBrush = new TextureBrush(watermark.ToBitmap()))
            {
                int x = (original.Width / 2) - (watermark.Width / 2);
                int y = (original.Height / 2) - (watermark.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                graphics.DrawImage(original, destRect);
                graphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermark.Width + 1, watermark.Height)));
            }

            return destImage;
        }

        /// <summary>
        /// Create a grayscale color palette.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <param name="numberOfEntries">The number of entries in the palette.</param>
        /// <returns>
        /// The grayscale color palette.
        /// </returns>
        public static ColorPalette CreateGrayscalePalette(PixelFormat format, int numberOfEntries)
        {
            // Make a new bitmap object to steal its palette
            using (Bitmap bitmap = new Bitmap(1, 1, format))
            {
                // Grab the palette
                ColorPalette palette = bitmap.Palette;

                // Populate the palette
                for (int i = 0; i < numberOfEntries; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }

                return palette;
            }
        }
    }
}
