using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Writers;
using AMI.Core.Mappers;
using AnimatedGif;
using RNS.Framework.Tools;

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

        /// <inheritdoc/>
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

            Ensure.ArgumentNotNull(mapper, nameof(mapper));
            Ensure.ArgumentNotNull(ct, nameof(ct));

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
