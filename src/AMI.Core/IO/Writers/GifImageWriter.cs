using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Extensions.Drawing;
using AMI.Core.Mappers;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace AMI.Core.IO.Writers
{
    /// <summary>
    /// A writer for GIF images.
    /// </summary>
    /// <seealso cref="IGifImageWriter" />
    public abstract class GifImageWriter : IGifImageWriter
    {
        private readonly string imageExtension = ImageFormat.Gif.FileExtensionFromEncoder();

        /// <summary>
        /// Initializes a new instance of the <see cref="GifImageWriter"/> class.
        /// </summary>
        public GifImageWriter()
        {
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<AxisContainerModel<string>>> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainerModel<string>> images,
            int delay,
            BezierEasingType bezierEasingType,
            CancellationToken ct)
        {
            try
            {
                var result = new List<AxisContainerModel<string>>();

                foreach (var axisImages in images.GroupBy(e => e.AxisType))
                {
                    ct.ThrowIfCancellationRequested();

                    string filename = await WriteAsync(
                        destinationPath,
                        axisImages.ToList(),
                        axisImages.Key.ToString(),
                        delay,
                        bezierEasingType,
                        ct);
                    result.Add(new AxisContainerModel<string>(axisImages.Key, filename));
                }

                return result;
            }
            catch (AmiException)
            {
                throw;
            }
            catch (OperationCanceledException e)
            {
                throw new AmiException("The writing of the GIF has been cancelled.", e);
            }
            catch (Exception e)
            {
                throw new AmiException("The GIF could not be written.", e);
            }
        }

        /// <inheritdoc/>
        public async Task<string> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainerModel<string>> images,
            string name,
            int delay,
            BezierEasingType bezierEasingType,
            CancellationToken ct)
        {
            try
            {
                var mapper = new BezierPositionMapper(Convert.ToUInt32(images.Count()), bezierEasingType);

                var filenames = images.OrderBy(e => e.AxisType).ThenBy(e => e.Position)
                    .Select(e => e.Entity).ToArray();

                var filename = $"{name}{imageExtension}";

                await AbstractWriteAsync(destinationPath, filename, destinationPath, filenames, delay, bezierEasingType, mapper, ct);

                return filename;
            }
            catch (OperationCanceledException e)
            {
                throw new AmiException("The writing of the GIF has been cancelled.", e);
            }
            catch (Exception e)
            {
                throw new AmiException("The GIF could not be written.", e);
            }
        }

        /// <summary>
        /// Writes the GIF images asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="destinationFilename">The destination filename.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="sourceFilenames">The source filenames.</param>
        /// <param name="delay">The delay between frames.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="mapper">The position mapper.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        protected abstract Task AbstractWriteAsync(
            string destinationPath,
            string destinationFilename,
            string sourcePath,
            string[] sourceFilenames,
            int delay,
            BezierEasingType bezierEasingType,
            BezierPositionMapper mapper,
            CancellationToken ct);
    }
}
