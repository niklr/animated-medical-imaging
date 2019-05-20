using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Enums;
using AMI.Core.Exceptions;
using AMI.Core.Extensions.Drawing;
using AMI.Core.Factories;
using AMI.Core.Mappers;
using AMI.Core.Models;
using AMI.Core.Readers;
using AMI.Core.Security;
using AMI.Core.Strategies;
using Microsoft.Extensions.Logging;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace AMI.Core.Extractors
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
        public ImageExtractor(ILoggerFactory loggerFactory, IFileSystemStrategy fileSystemStrategy, IImageReaderFactory<T1, T2> readerFactory)
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
        /// Extracts the images asynchronous.
        /// </summary>
        /// <param name="input">The input information.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The output of the image extraction.</returns>
        /// <exception cref="AmiException">Watermark could not be read.</exception>
        public async Task<ImageExtractOutput> ExtractAsync(ExtractInput input, CancellationToken ct)
        {
            var output = new ImageExtractOutput();
            var imageFormat = GetImageFormat(input.ImageFormat);
            var imageExtension = imageFormat.FileExtensionFromEncoder();
            var fs = fileSystemStrategy.Create(input.DestinationPath);
            var reader = readerFactory.Create();

            await reader.InitAsync(input.SourcePath, ct);

            reader.Mapper = new AxisPositionMapper(input.AmountPerAxis, reader.Width, reader.Height, reader.Depth);

            PreProcess(reader, imageFormat, input.AmountPerAxis, input.DesiredSize);

            output.LabelCount = reader.GetLabelCount();

            ISet<AxisType> axisTypes = new HashSet<AxisType>(input.AxisTypes);

            if (axisTypes.Count == 0)
            {
                axisTypes = reader.GetRecommendedAxisTypes();
            }

            BitmapContainer watermark = null;
            if (!string.IsNullOrWhiteSpace(input.WatermarkSourcePath))
            {
                BitmapReader bitmapReader = new BitmapReader();
                var watermarkBitmap = await bitmapReader.ReadAsync(input.WatermarkSourcePath, input.DesiredSize, ct);
                if (watermarkBitmap == null)
                {
                    throw new AmiException("Watermark could not be read.");
                }

                watermark = new BitmapContainer(watermarkBitmap);
            }

            ParallelOptions po = new ParallelOptions
            {
                CancellationToken = ct,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var images = new List<PositionAxisContainer<string>>();

            foreach (AxisType axisType in axisTypes)
            {
                Parallel.For(0, Convert.ToInt32(input.AmountPerAxis), po, i =>
                {
                    po.CancellationToken.ThrowIfCancellationRequested();

                    string filename = $"{axisType}_{i}{imageExtension}";
                    var bitmap = reader.ExtractPosition(axisType, Convert.ToUInt32(i), input.DesiredSize);
                    if (bitmap != null)
                    {
                        if (input.Grayscale)
                        {
                            bitmap = bitmap.To8bppIndexedGrayscale();
                        }

                        bitmap = bitmap.ToCenter(input.DesiredSize, Color.Black);

                        if (watermark != null)
                        {
                            bitmap = bitmap.AppendWatermark(watermark);
                        }

                        fs.File.WriteAllBytes(fs.Path.Combine(input.DestinationPath, filename), bitmap.ToByteArray(imageFormat));
                        images.Add(new PositionAxisContainer<string>(Convert.ToUInt32(i), axisType, filename));
                    }
                });
            }

            output.Images = images.OrderBy(e => e.Position).ToList();

            return output;
        }

        private void PreProcess(IImageReader<T2> reader, ImageFormat imageFormat, uint amount, uint? desiredSize)
        {
            if (amount > 1)
            {
                // calculate hashes of images for the defined amount for each axis
                IList<PositionAxisContainer<string>> hashes = new List<PositionAxisContainer<string>>();
                IDictionary<AxisType, uint[]> newMap = new Dictionary<AxisType, uint[]>();

                IAxisPositionMapper mapper = reader.Mapper;

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
                                string hash = Cryptography.CalculateSha1Hash(bitmap.ToByteArray(imageFormat));
                                hashes.Add(new PositionAxisContainer<string>(i, axisType, hash));
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
        private ImageFormat GetImageFormat(Enums.ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case Enums.ImageFormat.Jpeg:
                    return ImageFormat.Jpeg;
                case Enums.ImageFormat.Png:
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
