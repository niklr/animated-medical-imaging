using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Extensions.Drawing;
using AMI.Core.Extensions.FileSystemExtensions;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using itk.simple;

[assembly: InternalsVisibleTo("AMI.NetCore.Tests")]
[assembly: InternalsVisibleTo("AMI.NetFramework.Tests")]

namespace AMI.Itk.Utils
{
    /// <summary>
    /// An utility based on the Insight Segmentation and Registration Toolkit (ITK).
    /// </summary>
    internal class ItkUtil : IItkUtil
    {
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IFileExtensionMapper fileExtensionMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkUtil"/> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="fileExtensionMapper">The file extension mapper.</param>
        public ItkUtil(IFileSystemStrategy fileSystemStrategy, IFileExtensionMapper fileExtensionMapper)
            : base()
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.fileExtensionMapper = fileExtensionMapper ?? throw new ArgumentNullException(nameof(fileExtensionMapper));
        }

        /// <inheritdoc/>
        public ImageReaderBase CreateImageReader(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var fs = fileSystemStrategy.Create(path);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the provided path.");
            }

            if (fs.IsDirectory(path))
            {
                var entryPath = DiscoverEntryPath(fs.Directory.GetFiles(path));
                if (string.IsNullOrWhiteSpace(entryPath))
                {
                    // By default try to read DICOM image series
                    // https://simpleitk.readthedocs.io/en/master/Examples/DicomSeriesReader/Documentation.html
                    ImageSeriesReader seriesReader = new ImageSeriesReader();
                    seriesReader.SetFileNames(ImageSeriesReader.GetGDCMSeriesFileNames(path));
                    return seriesReader;
                }
                else
                {
                    // Try to read the image based on the entry path
                    ImageFileReader reader = new ImageFileReader();
                    reader.SetFileName(entryPath);
                    return reader;
                }
            }
            else
            {
                ImageFileReader reader = new ImageFileReader();
                reader.SetFileName(path);
                return reader;
            }
        }

        /// <inheritdoc/>
        public string DiscoverEntryPath(string[] files)
        {
            if (files != null)
            {
                var file = files.FirstOrDefault();
                if (file != null)
                {
                    // Check if directory contains multiple files
                    if (files.Length > 1)
                    {
                        var result = fileExtensionMapper.Map(file);
                        if (result.FileFormat == FileFormat.Dicom)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            if (result.FileExtensionType == FileExtensionType.Header)
                            {
                                return file;
                            }
                            else
                            {
                                var counterpart = fileExtensionMapper.MapCounterpart(file);
                                return files.Where(e => e.ToLowerInvariant().EndsWith(counterpart.Extension.ToLowerInvariant())).FirstOrDefault();
                            }
                        }
                    }
                    else
                    {
                        // Directory contains just 1 file
                        return file;
                    }
                }
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public async Task<Image> ReadImageAsync(string path, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var reader = CreateImageReader(path);
            if (reader == null)
            {
                throw new UnexpectedNullException("The image reader could not be created based on the provided path.");
            }

            try
            {
                ct.Register(() =>
                {
                    reader.Abort();
                });

                ct.ThrowIfCancellationRequested();

                Image image = reader.Execute();
                image = ApplyRescaleIntensityImageFilter(image);
                image = ApplyCastImageFilter(image);

                await Task.CompletedTask;

                return image;
            }
            catch (OperationCanceledException e)
            {
                throw new AmiException("The reading of the ITK image has been cancelled.", e);
            }
            catch (Exception e)
            {
                throw new AmiException("The ITK image could not be read.", e);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <inheritdoc/>
        public async Task WriteImageAsync(Image image, string path, string filename, CancellationToken ct)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var fs = fileSystemStrategy.Create(path);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the provided path.");
            }

            using (ImageFileWriter writer = new ImageFileWriter())
            {
                try
                {
                    ct.Register(() =>
                    {
                        writer.Abort();
                    });

                    ct.ThrowIfCancellationRequested();

                    writer.SetUseCompression(true);
                    writer.SetFileName(fs.Path.Combine(path, filename));
                    writer.Execute(image);

                    await Task.CompletedTask;
                }
                catch (OperationCanceledException e)
                {
                    throw new AmiException("The writing of the ITK image has been cancelled.", e);
                }
                catch (Exception e)
                {
                    throw new AmiException("The ITK image could not be written.", e);
                }
            }
        }

        /// <inheritdoc/>
        public Image ExtractPosition(Image image, AxisType axisType, uint index)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var size = image.GetSize();
            var indexVector = new VectorInt32 { 0, 0, 0 };
            int indexConverted = Convert.ToInt32(index);

            switch (axisType)
            {
                case AxisType.X:
                    size[0] = 0;
                    indexVector[0] = indexConverted;
                    break;
                case AxisType.Y:
                    size[1] = 0;
                    indexVector[1] = indexConverted;
                    break;
                case AxisType.Z:
                    size[2] = 0;
                    indexVector[2] = indexConverted;
                    break;
                default:
                    break;
            }

            ExtractImageFilter filter = new ExtractImageFilter();
            filter.SetSize(size);
            filter.SetIndex(indexVector);
            Image output = filter.Execute(image);
            filter.Dispose();

            return output ?? image;
        }

        /// <inheritdoc/>
        public Image ResampleImage2D(Image image, uint outputSize)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var dimension = image.GetDimension();
            if (dimension != 2)
            {
                throw new NotSupportedException($"The dimension ({dimension}) of the provided image is not supported.");
            }

            var inputSize = image.GetSize();
            var inputSpacing = image.GetSpacing();
            var inputDirection = image.GetDirection();
            var inputOrigin = image.GetOrigin();

            double oldw = inputSize[0] * inputSpacing[0];
            double oldh = inputSize[1] * inputSpacing[1];
            double neww, newh = 0;
            double rw = oldw / outputSize;
            double rh = oldh / outputSize;

            if (rw > rh)
            {
                newh = oldh / rw;
                neww = outputSize;
            }
            else
            {
                neww = oldw / rh;
                newh = outputSize;
            }

            var outputSpacing = new VectorDouble()
            {
                inputSpacing[0] * inputSize[0] / neww,
                inputSpacing[1] * inputSize[1] / newh
            };

            var actualSize = new VectorUInt32()
            {
                (uint)neww,
                (uint)newh
            };

            // https://itk.org/ITKExamples/src/Filtering/ImageGrid/ResampleAnImage/Documentation.html
            ResampleImageFilter filter = new ResampleImageFilter();
            filter.SetReferenceImage(image);
            filter.SetSize(actualSize);
            filter.SetOutputSpacing(outputSpacing);
            Image output = filter.Execute(image);
            filter.Dispose();

            return output ?? image;
        }

        /// <inheritdoc/>
        public Image ResampleImage3D(Image image, uint outputSize)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var dimension = image.GetDimension();
            if (dimension != 3)
            {
                throw new NotSupportedException($"The dimension ({dimension}) of the provided image is not supported.");
            }

            var inputSize = image.GetSize();
            var inputSpacing = image.GetSpacing();

            var desiredSize = new VectorUInt32()
            {
                outputSize,
                outputSize,
                outputSize
            };

            var outputSpacing = new VectorDouble()
            {
                inputSpacing[0] * inputSize[0] / desiredSize[0],
                inputSpacing[1] * inputSize[1] / desiredSize[1],
                inputSpacing[2] * inputSize[2] / desiredSize[2]
            };

            // https://itk.org/Wiki/ITK/Examples/ImageProcessing/ResampleImageFilter
            ResampleImageFilter filter = new ResampleImageFilter();
            filter.SetReferenceImage(image);
            filter.SetTransform(new ScaleTransform(image.GetDimension()));
            filter.SetInterpolator(InterpolatorEnum.sitkLinear);
            filter.SetSize(desiredSize);
            filter.SetOutputSpacing(outputSpacing);
            Image output = filter.Execute(image);
            filter.Dispose();

            return output ?? image;
        }

        /// <inheritdoc/>
        public ulong GetLabelCount(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            PixelIDValueEnum imageType = PixelIDValueEnum.swigToEnum(image.GetPixelIDValue());
            Image labelImage = new Image(image.GetWidth(), image.GetHeight(), image.GetDepth(), imageType);

            labelImage.SetOrigin(image.GetOrigin());
            labelImage.SetDirection(image.GetDirection());
            labelImage.SetSpacing(image.GetSpacing());

            LabelStatisticsImageFilter filter = new LabelStatisticsImageFilter();
            filter.Execute(labelImage, image);
            ulong labelCount = filter.GetNumberOfLabels();
            filter.Dispose();

            return labelCount;
        }

        /// <inheritdoc/>
        public System.Drawing.Bitmap ToBitmap(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var dimension = image.GetDimension();
            if (dimension != 2)
            {
                throw new NotSupportedException($"The dimension ({dimension}) of the provided image is not supported.");
            }

            int height = Convert.ToInt32(image.GetHeight());
            int width = Convert.ToInt32(image.GetWidth());

            return GetBuffer(image).ToBitmap(width, height);
        }

        private Image ApplyRescaleIntensityImageFilter(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            Image output = null;

            try
            {
                // Execute rescale intensity image filter
                RescaleIntensityImageFilter filter = new RescaleIntensityImageFilter();
                filter.SetOutputMinimum(0);
                filter.SetOutputMaximum(255);
                output = filter.Execute(image);
                filter.Dispose();
            }
            catch (Exception)
            {
                // ignore "RescaleIntensityImageFilter(000000F79D6CFC40):
                // Minimum output value cannot be greater than Maximum output value." exceptions
            }

            return output ?? image;
        }

        private Image ApplyCastImageFilter(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // TODO: find mapping between itk.simple.PixelIDValueEnum and System.Drawing.Imaging.PixelFormat
            // see https://github.com/SimpleITK/SimpleITK/issues/582

            // Execute cast filter
            CastImageFilter filter = new CastImageFilter();
            PixelIDValueEnum imageType = PixelIDValueEnum.swigToEnum(image.GetPixelIDValue());
            if (imageType.ToString().ToLowerInvariant().Contains("vector"))
            {
                filter.SetOutputPixelType(PixelIDValueEnum.sitkVectorUInt8);
            }
            else
            {
                filter.SetOutputPixelType(PixelIDValueEnum.sitkUInt8);
            }

            Image output = filter.Execute(image);
            filter.Dispose();

            return output ?? image;
        }

        private IntPtr GetBuffer(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            PixelIDValueEnum imageType = PixelIDValueEnum.swigToEnum(image.GetPixelIDValue());
            int swigValue = imageType.swigValue;

            if (swigValue == PixelIDValueEnum.sitkUInt8.swigValue || swigValue == PixelIDValueEnum.sitkVectorUInt8.swigValue)
            {
                return image.GetBufferAsUInt8();
            }
            else if (swigValue == PixelIDValueEnum.sitkUInt16.swigValue || swigValue == PixelIDValueEnum.sitkVectorUInt16.swigValue)
            {
                return image.GetBufferAsUInt16();
            }
            else if (swigValue == PixelIDValueEnum.sitkUInt32.swigValue || swigValue == PixelIDValueEnum.sitkVectorUInt32.swigValue)
            {
                return image.GetBufferAsUInt32();
            }
            else if (swigValue == PixelIDValueEnum.sitkUInt64.swigValue || swigValue == PixelIDValueEnum.sitkVectorUInt64.swigValue)
            {
                throw new NotSupportedException($"The image type {imageType} is not supported.");
            }
            else if (swigValue == PixelIDValueEnum.sitkInt8.swigValue || swigValue == PixelIDValueEnum.sitkVectorInt8.swigValue)
            {
                return image.GetBufferAsInt8();
            }
            else if (swigValue == PixelIDValueEnum.sitkInt16.swigValue || swigValue == PixelIDValueEnum.sitkVectorInt16.swigValue)
            {
                return image.GetBufferAsInt16();
            }
            else if (swigValue == PixelIDValueEnum.sitkInt32.swigValue || swigValue == PixelIDValueEnum.sitkVectorInt32.swigValue)
            {
                return image.GetBufferAsInt32();
            }
            else if (swigValue == PixelIDValueEnum.sitkInt64.swigValue || swigValue == PixelIDValueEnum.sitkVectorInt64.swigValue)
            {
                throw new NotSupportedException($"The image type {imageType} is not supported.");
            }
            else if (swigValue == PixelIDValueEnum.sitkFloat32.swigValue || swigValue == PixelIDValueEnum.sitkComplexFloat32.swigValue)
            {
                return image.GetBufferAsFloat();
            }
            else if (swigValue == PixelIDValueEnum.sitkFloat64.swigValue || swigValue == PixelIDValueEnum.sitkComplexFloat64.swigValue)
            {
                return image.GetBufferAsDouble();
            }
            else
            {
                throw new NotSupportedException($"The image type {imageType} is not supported.");
            }
        }
    }
}
