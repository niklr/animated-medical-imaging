using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Abstractions;
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
using RNS.Framework.Tools;
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

            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.readerFactory = readerFactory ?? throw new ArgumentNullException(nameof(readerFactory));
        }

        /// <inheritdoc/>
        public async Task<ProcessResultModel> ProcessAsync(ProcessPathCommand command, CancellationToken ct)
        {
            Ensure.ArgumentNotNull(command, nameof(command));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            ImageFormat imageFormat = GetImageFormat(command.ImageFormat);
            string imageExtension = imageFormat.FileExtensionFromEncoder();
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

            PreProcess(reader, imageFormat, command.AmountPerAxis, command.OutputSize);

            var result = new ProcessResultModel
            {
                LabelCount = Convert.ToInt32(reader.GetLabelCount()),
                Size = new int[] { reader.Width, reader.Height, reader.Depth }
            };

            ISet<AxisType> axisTypes = new HashSet<AxisType>(command.AxisTypes);
            if (axisTypes.Count == 0)
            {
                axisTypes = new HashSet<AxisType> { AxisType.Z };
            }

            BitmapWrapper watermark = null;
            if (!string.IsNullOrWhiteSpace(command.WatermarkSourcePath))
            {
                BitmapReader bitmapReader = new BitmapReader();
                var watermarkBitmap = await bitmapReader.ReadAsync(command.WatermarkSourcePath, command.OutputSize, ct);
                if (watermarkBitmap == null)
                {
                    throw new UnexpectedNullException("Watermark could not be read.");
                }

                watermark = new BitmapWrapper(watermarkBitmap);
            }

            var images = new List<PositionAxisContainerModel<string>>();

            foreach (AxisType axisType in axisTypes)
            {
                for (int i = 0; i < reader.Mapper.GetLength(axisType); i++)
                {
                    ct.ThrowIfCancellationRequested();

                    string filename = $"{axisType}_{i}{imageExtension}";

                    var image = WriteImage(i, fs, command, reader, axisType, imageFormat, filename, watermark);
                    if (image != null)
                    {
                        images.Add(image);
                    }
                }
            }

            reader.Dispose();

            result.Images = images.OrderBy(e => e.Position).ToList();

            return result;
        }

        private void PreProcess(IImageReader<T2> reader, ImageFormat imageFormat, int amount, int? outputSize)
        {
            Ensure.ArgumentNotNull(reader, nameof(reader));

            if (amount > 1)
            {
                // count labels of images for the defined amount for each axis
                IList<PositionAxisContainerModel<ulong>> labels = new List<PositionAxisContainerModel<ulong>>();
                IDictionary<AxisType, int[]> newMap = new Dictionary<AxisType, int[]>();

                IAxisPositionMapper mapper = reader.Mapper;
                if (mapper == null)
                {
                    throw new UnexpectedNullException("The image reader does not contain a mapper.");
                }

                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    int length = mapper.GetLength(axisType);
                    newMap[axisType] = new int[Math.Min(amount, length)];

                    for (int i = 0; i < newMap[axisType].Length; i++)
                    {
                        // set initial mapped positions
                        newMap[axisType][i] = mapper.GetMappedPosition(axisType, i);

                        var labelCount = reader.GetLabelCount(axisType, i);
                        labels.Add(new PositionAxisContainerModel<ulong>(i, axisType, labelCount));
                    }
                }

                // calculate new mapped positions based on the labels
                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    var candidates = labels
                        .Where(e => e.AxisType == axisType)
                        .Where(e => e.Entity > 1)
                        .OrderBy(e => e.Position);

                    var startPosition = candidates.FirstOrDefault();
                    var endPosition = candidates.LastOrDefault();

                    if (startPosition != null && endPosition != null)
                    {
                        if (startPosition == endPosition)
                        {
                            newMap[axisType] = new int[1];
                            newMap[axisType][0] = mapper.GetMappedPosition(axisType, startPosition.Position);
                        }
                        else
                        {
                            // calculate mappedPosition between startPosition and endPosition
                            int mappedStartPosition = mapper.GetMappedPosition(axisType, startPosition.Position);
                            int mappedEndPosition = mapper.GetMappedPosition(axisType, endPosition.Position);
                            int length = mappedEndPosition - mappedStartPosition;

                            if (length >= newMap[axisType].Length)
                            {
                                for (int i = 0; i < newMap[axisType].Length; i++)
                                {
                                    newMap[axisType][i] = mappedStartPosition + mapper.CalculateMappedPosition(amount, length, i);
                                }
                            }
                        }
                    }
                }

                // set a new mapper with the new map
                reader.Mapper = new AxisPositionMapper(newMap);
            }
        }

        private PositionAxisContainerModel<string> WriteImage(
            int position,
            IFileSystem fs,
            ProcessPathCommand command,
            T1 reader,
            AxisType axisType,
            ImageFormat imageFormat,
            string filename,
            BitmapWrapper watermark = null)
        {
            var bitmap = reader.ExtractPosition(axisType, position, command.OutputSize);
            if (bitmap != null)
            {
                if (command.Grayscale)
                {
                    bitmap = bitmap.To8bppIndexedGrayscale();
                }

                bitmap = bitmap.ToCenter(command.OutputSize, Color.Black);
                if (bitmap == null)
                {
                    throw new UnexpectedNullException("Bitmap could not be centered.");
                }

                if (watermark != null)
                {
                    bitmap = bitmap.AppendWatermark(watermark);
                }

                fs.File.WriteAllBytes(fs.Path.Combine(command.DestinationPath, filename), bitmap.ToByteArray(imageFormat));

                bitmap.Dispose();

                return new PositionAxisContainerModel<string>(position, axisType, filename);
            }

            return null;
        }

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
