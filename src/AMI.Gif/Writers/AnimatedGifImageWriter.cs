using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Writers;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Domain.Exceptions;
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
        private readonly IFileSystemStrategy fileSystemStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedGifImageWriter" /> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public AnimatedGifImageWriter(IFileSystemStrategy fileSystemStrategy)
            : base()
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
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
            Ensure.ArgumentNotNullOrWhiteSpace(destinationPath, nameof(destinationPath));
            Ensure.ArgumentNotNullOrWhiteSpace(destinationFilename, nameof(destinationFilename));
            Ensure.ArgumentNotNullOrWhiteSpace(sourcePath, nameof(sourcePath));

            Ensure.ArgumentNotNull(sourceFilenames, nameof(sourceFilenames));
            Ensure.ArgumentNotNull(mapper, nameof(mapper));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            var fs = fileSystemStrategy.Create(destinationPath);
            if (fs == null)
            {
                throw new UnexpectedNullException(
                    $"Filesystem could not be created based on the destination path '{destinationPath}'.");
            }

            await Task.Run(
                () =>
                {
                    // 33ms delay (~30fps)
                    using (var gif = AnimatedGif.AnimatedGif.Create(fs.Path.Combine(destinationPath, destinationFilename), 33))
                    {
                        for (uint i = 0; i < sourceFilenames.Length; i++)
                        {
                            ct.ThrowIfCancellationRequested();

                            using (Image image = Image.FromFile(fs.Path.Combine(sourcePath, sourceFilenames[i])))
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
