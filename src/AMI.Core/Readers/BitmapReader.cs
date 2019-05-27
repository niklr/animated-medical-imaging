using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Extensions.Drawing;
using AMI.Domain.Exceptions;

namespace AMI.Core.Readers
{
    /// <summary>
    /// A reader for bitmaps.
    /// </summary>
    public class BitmapReader
    {
        /// <summary>
        /// Reads the image as bitmap asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="desiredSize">The desired size.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The image as bitmap.</returns>
        public async Task<Bitmap> ReadAsync(string path, uint? desiredSize, CancellationToken ct)
        {
            return await Task.Run(
                () =>
                {
                    try
                    {
                        ct.ThrowIfCancellationRequested();

                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            Image image = Image.FromFile(path);

                            if (image != null)
                            {
                                Bitmap bitmap = new Bitmap(image);
                                return bitmap.Resize(desiredSize);
                            }
                        }
                        return null;
                    }
                    catch (OperationCanceledException e)
                    {
                        throw new AmiException("The reading of the bitmap has been cancelled.", e);
                    }
                    catch (Exception e)
                    {
                        throw new AmiException("The bitmap could not be read.", e);
                    }
                }, ct);
        }
    }
}
