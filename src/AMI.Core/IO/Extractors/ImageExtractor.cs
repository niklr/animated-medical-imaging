using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessPath;
using AMI.Core.Extensions.Drawing;
using AMI.Core.Factories;
using AMI.Core.IO.Readers;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Core.Wrappers;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using RNS.Framework.Security;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace AMI.Core.IO.Extractors
{
    /// <summary>
    /// An extractor for images.
    /// </summary>
    /// <typeparam name="T1">The type of the reader.</typeparam>
    /// <typeparam name="T2">The type of the image.</typeparam>
    /// <seealso cref="IImageExtractor" />
    public abstract class ImageExtractor<T1, T2> : IImageExtractor
        where T1 : IImageReader<T2>
    {
        private readonly ILogger logger;
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IImageReaderFactory<T1, T2> readerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExtractor{T1, T2}"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="readerFactory">The image reader factory.</param>
        /// <exception cref="ArgumentNullException">
        /// loggerFactory
        /// or
        /// fileSystemStrategy
        /// or
        /// readerFactory
        /// </exception>
        public ImageExtractor(
            ILoggerFactory loggerFactory,
            IFileSystemStrategy fileSystemStrategy,
            IImageReaderFactory<T1, T2> readerFactory)
        {
            logger = loggerFactory?.CreateLogger<ImageExtractor<T1, T2>>();
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.fileSystemStrategy = fileSystemStrategy;
            if (this.fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            this.readerFactory = readerFactory;
            if (this.readerFactory == null)
            {
                throw new ArgumentNullException(nameof(readerFactory));
            }
        }

        /// <summary>
        /// Processes the images asynchronous.
        /// </summary>
        /// <param name="command">The command information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The result of the image processing.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// command
        /// or
        /// ct
        /// </exception>
        /// <exception cref="UnexpectedNullException">
        /// Image file extension could not be determined.
        /// or
        /// Filesystem could not be created based on the destination path.
        /// or
        /// Image reader could not be created.
        /// or
        /// Watermark could not be read.
        /// or
        /// Bitmap could not be centered.
        /// </exception>
        public async Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var result = new ProcessResultModel();
            var imageFormat = GetImageFormat(command.ImageFormat);
            var imageExtension = imageFormat.FileExtensionFromEncoder();
            if (string.IsNullOrWhiteSpace(imageExtension))
            {
                throw new UnexpectedNullException("Image file extension could not be determined.");
            }

            var fs = fileSystemStrategy.Create(command.DestinationPath);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the destination path.");
            }

            var reader = readerFactory.Create();
            if (reader == null)
            {
                throw new UnexpectedNullException("Image reader could not be created.");
            }

            await reader.InitAsync(command.SourcePath, ct);

            reader.Mapper = new AxisPositionMapper(command.AmountPerAxis, reader.Width, reader.Height, reader.Depth);

            PreProcess(reader, imageFormat, command.AmountPerAxis, command.DesiredSize);

            result.LabelCount = Convert.ToInt32(reader.GetLabelCount());

            ISet<AxisType> axisTypes = new HashSet<AxisType>(command.AxisTypes);

            if (axisTypes.Count == 0)
            {
                axisTypes = reader.GetRecommendedAxisTypes();
            }

            BitmapWrapper watermark = null;
            if (!string.IsNullOrWhiteSpace(command.WatermarkSourcePath))
            {
                BitmapReader bitmapReader = new BitmapReader();
                var watermarkBitmap = await bitmapReader.ReadAsync(command.WatermarkSourcePath, command.DesiredSize, ct);
                if (watermarkBitmap == null)
                {
                    throw new UnexpectedNullException("Watermark could not be read.");
                }

                watermark = new BitmapWrapper(watermarkBitmap);
            }

            var images = new List<PositionAxisContainerModel<string>>();

            foreach (AxisType axisType in axisTypes)
            {
                for (int i = 0; i < command.AmountPerAxis; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    string filename = $"{axisType}_{i}{imageExtension}";
                    var bitmap = reader.ExtractPosition(axisType, Convert.ToUInt32(i), command.DesiredSize);
                    if (bitmap != null)
                    {
                        if (command.Grayscale)
                        {
                            bitmap = bitmap.To8bppIndexedGrayscale();
                        }

                        bitmap = bitmap.ToCenter(command.DesiredSize, Color.Black);
                        if (bitmap == null)
                        {
                            throw new UnexpectedNullException("Bitmap could not be centered.");
                        }

                        if (watermark != null)
                        {
                            bitmap = bitmap.AppendWatermark(watermark);
                        }

                        fs.File.WriteAllBytes(fs.Path.Combine(command.DestinationPath, filename), bitmap.ToByteArray(imageFormat));
                        images.Add(new PositionAxisContainerModel<string>(Convert.ToUInt32(i), axisType, filename));
                    }
                }
            }

            result.Images = images.OrderBy(e => e.Position).ToList();

            return result;
        }

        private void PreProcess(IImageReader<T2> reader, ImageFormat imageFormat, uint amount, uint? desiredSize)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (amount > 1)
            {
                // calculate hashes of images for the defined amount for each axis
                IList<PositionAxisContainerModel<string>> hashes = new List<PositionAxisContainerModel<string>>();
                IDictionary<AxisType, uint[]> newMap = new Dictionary<AxisType, uint[]>();

                IAxisPositionMapper mapper = reader.Mapper;
                if (mapper == null)
                {
                    throw new UnexpectedNullException("The image reader does not contain a mapper.");
                }

                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    newMap[axisType] = new uint[amount];

                    for (uint i = 0; i < amount; i++)
                    {
                        // set initial mapped positions
                        newMap[axisType][i] = mapper.GetMappedPosition(axisType, i);

                        using (var bitmap = reader.ExtractPosition(axisType, i, desiredSize))
                        {
                            if (bitmap != null)
                            {
                                string hash = Cryptography.CalculateSHA1Hash(bitmap.ToByteArray(imageFormat));
                                hashes.Add(new PositionAxisContainerModel<string>(i, axisType, hash));
                            }
                        }
                    }
                }

                // calculate new mapped positions based on the hashes
                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    var axisHashes = hashes.Where(e => e.AxisType == axisType);

                    // find the most likely hash representing an empty image
                    string emptyHash = axisHashes.GroupBy(e => e.Entity)
                        .Where(e => e.Count() > 1).OrderBy(e => e.Count()).Select(e => e.Key).FirstOrDefault();

                    if (emptyHash != null)
                    {
                        var nonEmptyHashes = axisHashes.Where(e => e.Entity != emptyHash).OrderBy(e => e.Position);

                        var startPosition = nonEmptyHashes.FirstOrDefault();
                        var endPosition = nonEmptyHashes.LastOrDefault();

                        if (startPosition != null && endPosition != null)
                        {
                            if (startPosition == endPosition)
                            {
                                // set the same mappedPosition for all positions
                                for (int i = 0; i < amount; i++)
                                {
                                    newMap[axisType][i] = mapper.GetMappedPosition(axisType, startPosition.Position);
                                }
                            }
                            else
                            {
                                // calculate mappedPosition between startPosition and endPosition
                                uint mappedStartPosition = mapper.GetMappedPosition(axisType, startPosition.Position);
                                uint mappedEndPosition = mapper.GetMappedPosition(axisType, endPosition.Position);
                                uint length = mappedEndPosition - mappedStartPosition;

                                for (uint i = 0; i < amount; i++)
                                {
                                    newMap[axisType][i] = mappedStartPosition + mapper.CalculateMappedPosition(amount, length, i);
                                }
                            }
                        }
                    }
                }

                // TODO: distribute equally (newMap) to avoid re-use of the same image excessively compared to others

                /*
                 * Request:
                 * - Extract x images and each image at least once.
                 * Special case:
                 * - Less than x images can be extracted.
                 * Preferred behaviour:
                 * - Prioritize images in the middle for re-use.
                 */

                // set a new mapper with the new map
                reader.Mapper = new AxisPositionMapper(newMap);
            }
        }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        /// <returns>The <see cref="ImageFormat"/>.</returns>
        private ImageFormat GetImageFormat(Domain.Enums.ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case Domain.Enums.ImageFormat.Jpeg:
                    return ImageFormat.Jpeg;
                case Domain.Enums.ImageFormat.Png:
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
