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
        /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
        protected override async Task AbstractWriteAsync(
            string destinationPath,
            string destinationFilename,
            string sourcePath,
            string[] sourceFilenames,
            BezierPositionMapper mapper,
            CancellationToken ct)
        {
            await Task.Run(() =>
            {
                // 33ms delay (~30fps)
                using (var gif = AnimatedGif.AnimatedGif.Create(Path.Combine(destinationPath, destinationFilename), 33))
                {
                    for (uint i = 0; i < sourceFilenames.Length; i++)
                    {
                        ct.ThrowIfCancellationRequested();

                        using (Image image = Image.FromFile(Path.Combine(sourcePath, sourceFilenames[i])))
                        {
                            int delay = mapper == null ? -1 : Convert.ToInt32(mapper.GetMappedPosition(i));
                            gif.AddFrame(image, delay, quality: GifQuality.Bit8);
                        }
                    }
                }
            });
        }
    }
}
