﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Enums;
using AMI.Core.Exceptions;
using AMI.Core.Extensions.Drawing;
using AMI.Core.Mappers;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace AMI.Core.Writers
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

        /// <summary>
        /// Writes the GIF images asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="images">The images.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The output information for each axis.
        /// </returns>
        /// <exception cref="AmiException">
        /// The writing of the GIF has been cancelled.
        /// or
        /// The GIF could not be written.
        /// </exception>
        public async Task<IReadOnlyList<AxisContainer<string>>> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainer<string>> images,
            BezierEasingType bezierEasingType,
            CancellationToken ct)
        {
            try
            {
                var result = new List<AxisContainer<string>>();

                foreach (var axisImages in images.GroupBy(e => e.AxisType))
                {
                    ct.ThrowIfCancellationRequested();
                    string filename = await WriteAsync(
                        destinationPath,
                        axisImages.ToList(),
                        axisImages.Key.ToString(),
                        bezierEasingType,
                        ct);
                    result.Add(new AxisContainer<string>(axisImages.Key, filename));
                }

                return result;
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
        /// Writes the GIF image asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="images">The images.</param>
        /// <param name="name">The name of the GIF image without file extension.</param>
        /// <param name="bezierEasingType">Type of the bezier easing.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The filename of the GIF image.
        /// </returns>
        /// <exception cref="AmiException">
        /// The writing of the GIF has been cancelled.
        /// or
        /// The GIF could not be written.
        /// </exception>
        public async Task<string> WriteAsync(
            string destinationPath,
            IReadOnlyList<PositionAxisContainer<string>> images,
            string name,
            BezierEasingType bezierEasingType,
            CancellationToken ct)
        {
            try
            {
                var mapper = new BezierPositionMapper(Convert.ToUInt32(images.Count()), bezierEasingType);

                var filenames = images.OrderBy(e => e.AxisType).ThenBy(e => e.Position)
                    .Select(e => e.Entity).ToArray();

                var filename = $"{name}{imageExtension}";

                await AbstractWriteAsync(destinationPath, filename, destinationPath, filenames, mapper, ct);

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
        /// <param name="mapper">The position mapper.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// destinationPath
        /// or
        /// destinationFilename
        /// or
        /// sourcePath
        /// or
        /// sourceFilenames
        /// or
        /// mapper
        /// or
        /// ct
        /// </exception>
        protected abstract Task AbstractWriteAsync(
            string destinationPath,
            string destinationFilename,
            string sourcePath,
            string[] sourceFilenames,
            BezierPositionMapper mapper,
            CancellationToken ct);
    }
}
