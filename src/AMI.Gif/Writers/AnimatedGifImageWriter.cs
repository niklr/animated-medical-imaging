using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Mappers;
using AMI.Core.Writers;
using AnimatedGif;

namespace AMI.Gif.Writers
{
    /// <summary>
    /// A writer for GIF images.
    /// </summary>
    /// <seealso cref="GifImageWriter" />
    public class AnimatedGifImageWriter : GifImageWriter, IAnimatedGifImageWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedGifImageWriter"/> class.
        /// </summary>
        public AnimatedGifImageWriter()
            : base()
        {
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
        protected override async Task AbstractWriteAsync(
            string destinationPath,
            string destinationFilename,
            string sourcePath,
            string[] sourceFilenames,
            BezierPositionMapper mapper,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (string.IsNullOrWhiteSpace(destinationFilename))
            {
                throw new ArgumentNullException(nameof(destinationFilename));
            }

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (sourceFilenames == null)
            {
                throw new ArgumentNullException(nameof(sourceFilenames));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            await Task.Run(
                () =>
                {
                    // 33ms delay (~30fps)
                    using (var gif = AnimatedGif.AnimatedGif.Create(Path.Combine(destinationPath, destinationFilename), 33))
                    {
                        for (uint i = 0; i < sourceFilenames.Length; i++)
                        {
                            ct.ThrowIfCancellationRequested();

                            using (Image image = Image.FromFile(Path.Combine(sourcePath, sourceFilenames[i])))
                            {
                                int delay = Convert.ToInt32(mapper.GetMappedPosition(i));
                                gif.AddFrame(image, delay, quality: GifQuality.Bit8);
                            }
                        }
                    }
                }, ct);
        }
    }
}
