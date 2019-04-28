using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Enums;
using AMI.Core.Exceptions;
using AMI.Core.Extensions.Drawing;
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
    /// <typeparam name="T">The type of image.</typeparam>
    /// <seealso cref="IImageExtractor" />
    public abstract class ImageExtractor<T> : IImageExtractor
    {
        private readonly ILogger logger;
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IImageReader<T> reader;

        private uint? desiredSize = null;
        private ISet<AxisType> axisTypes = new HashSet<AxisType>();
        private ImageFormat imageFormat = ImageFormat.Png;
        private string imageExtension = ImageFormat.Png.FileExtensionFromEncoder();
        private bool grayscale = true;
        private string watermarkPath = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExtractor{T}"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="reader">The image reader.</param>
        /// <exception cref="ArgumentNullException">
        /// loggerFactory
        /// or
        /// fileSystemStrategy
        /// or
        /// reader
        /// </exception>
        public ImageExtractor(ILoggerFactory loggerFactory, IFileSystemStrategy fileSystemStrategy, IImageReader<T> reader)
        {
            logger = loggerFactory?.CreateLogger<ImageExtractor<T>>();
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.fileSystemStrategy = fileSystemStrategy;
            if (this.fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }

            this.reader = reader;
            if (this.reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            logger.LogInformation("ImageExtractor created");
        }

        /// <summary>
        /// Sets the desired size.
        /// </summary>
        /// <param name="desiredSize">The desired size.</param>
        public void SetDesiredSize(uint desiredSize)
        {
            this.desiredSize = desiredSize;
        }

        /// <summary>
        /// Sets the axis types.
        /// </summary>
        /// <param name="axisTypes">The axis types.</param>
        public void SetAxisTypes(params AxisType[] axisTypes)
        {
            this.axisTypes = new HashSet<AxisType>(axisTypes);
        }

        /// <summary>
        /// Sets the image format.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        public void SetImageFormat(Enums.ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case Enums.ImageFormat.Jpeg:
                    this.imageFormat = ImageFormat.Jpeg;
                    break;
                case Enums.ImageFormat.Png:
                default:
                    this.imageFormat = ImageFormat.Png;
                    break;
            }

            imageExtension = this.imageFormat.FileExtensionFromEncoder();
        }

        /// <summary>
        /// Sets whether the images should be converted to grayscale.
        /// </summary>
        /// <param name="grayscale">if set to <c>true</c> convert the images to grayscale.</param>
        public void SetGrayscale(bool grayscale)
        {
            this.grayscale = grayscale;
        }

        /// <summary>
        /// Sets the path pointing to the watermark.
        /// </summary>
        /// <param name="path">The path pointing to the watermark.</param>
        public void SetWatermarkPath(string path)
        {
            watermarkPath = path;
        }

        /// <summary>
        /// Extracts the images asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="amount">The amount of images to be extracted.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The output of the image extraction.</returns>
        /// <exception cref="AmiException">Watermark could not be read.</exception>
        public async Task<ImageExtractOutput> ExtractAsync(string sourcePath, string destinationPath, uint amount, CancellationToken ct)
        {
            var output = new ImageExtractOutput();
            var fs = fileSystemStrategy.Create(destinationPath);

            await reader.InitAsync(sourcePath, ct);
            reader.Mapper = new AxisPositionMapper(amount, reader.Width, reader.Height, reader.Depth);

            PreProcess(reader, amount);

            output.LabelCount = reader.GetLabelCount();

            if (axisTypes.Count == 0)
            {
                axisTypes = reader.GetRecommendedAxisTypes();
            }

            BitmapContainer watermark = null;
            if (!string.IsNullOrWhiteSpace(watermarkPath))
            {
                BitmapReader bitmapReader = new BitmapReader();
                var watermarkBitmap = await bitmapReader.ReadAsync(watermarkPath, desiredSize, ct);
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
                Parallel.For(0, Convert.ToInt32(amount), po, i =>
                {
                    po.CancellationToken.ThrowIfCancellationRequested();

                    string filename = $"{axisType}_{i}{imageExtension}";
                    var bitmap = reader.ExtractPosition(axisType, Convert.ToUInt32(i), desiredSize);
                    if (bitmap != null)
                    {
                        if (grayscale)
                        {
                            bitmap = bitmap.To8bppIndexedGrayscale();
                        }

                        bitmap = bitmap.ToCenter(desiredSize, Color.Black);

                        if (watermark != null)
                        {
                            bitmap = bitmap.AppendWatermark(watermark);
                        }

                        fs.File.WriteAllBytes(fs.Path.Combine(destinationPath, filename), bitmap.ToByteArray(imageFormat));
                        images.Add(new PositionAxisContainer<string>(Convert.ToUInt32(i), axisType, filename));
                    }
                });
            }

            output.Images = images;

            return output;
        }

        private void PreProcess(IImageReader<T> reader, uint amount)
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
    }
}
