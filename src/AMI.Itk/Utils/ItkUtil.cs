using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Enums;
using AMI.Core.Exceptions;
using AMI.Core.Extensions.Drawing;
using itk.simple;

namespace AMI.Itk.Utils
{
    /// <summary>
    /// An utility based on the Insight Segmentation and Registration Toolkit (ITK).
    /// </summary>
    internal class ItkUtil : IItkUtil
    {
        /// <summary>
        /// Reads the image asynchronous.
        /// </summary>
        /// <param name="path">The location of the image on the file system.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// The ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// path
        /// or
        /// ct
        /// </exception>
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

            return await Task.Run(
                () =>
                {
                    using (ImageFileReader reader = new ImageFileReader())
                    {
                        try
                        {
                            ct.Register(() =>
                            {
                                reader.Abort();
                            });

                            ct.ThrowIfCancellationRequested();

                            reader.SetFileName(path);

                            Image image = reader.Execute();
                            image = ApplyRescaleIntensityImageFilter(image);
                            image = ApplyCastImageFilter(image);
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
                    }
                }, ct);
        }

        /// <summary>
        /// Writes the image asynchronous.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <param name="path">The file system location where the image should be written.</param>
        /// <param name="filename">The name of the file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// image
        /// or
        /// path
        /// or
        /// filename
        /// or
        /// ct
        /// </exception>
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

            await Task.Run(
                () =>
                    {
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
                                writer.SetFileName(Path.Combine(path, filename));
                                writer.Execute(image);
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
                    }, ct);
        }

        /// <summary>
        /// Extracts the position on the specified axis.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="index">The position.</param>
        /// <returns>
        /// The extracted position as two-dimensional ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
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

            var filter = new ExtractImageFilter();
            filter.SetSize(size);
            filter.SetIndex(indexVector);

            return filter.Execute(image);
        }

        /// <summary>
        /// Resamples the two-dimensional ITK image to the desired size.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image.</param>
        /// <param name="desiredSize">The desired size.</param>
        /// <returns>
        /// The resampled two-dimensional ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        /// <exception cref="NotSupportedException">The dimension ({dimension}) of the provided image is not supported.</exception>
        public Image ResampleImage2D(Image image, uint desiredSize)
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

            var outputSize = new VectorUInt32()
            {
                desiredSize,
                desiredSize
            };

            double oldw = inputSize[0] * inputSpacing[0];
            double oldh = inputSize[1] * inputSpacing[1];
            double neww, newh = 0;
            double rw = oldw / desiredSize;
            double rh = oldh / desiredSize;

            if (rw > rh)
            {
                newh = oldh / rw;
                neww = desiredSize;
            }
            else
            {
                neww = oldw / rh;
                newh = desiredSize;
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
            var resampleFilter = new ResampleImageFilter();
            resampleFilter.SetReferenceImage(image);
            resampleFilter.SetSize(actualSize);
            resampleFilter.SetOutputSpacing(outputSpacing);

            var outputImage = resampleFilter.Execute(image);

            return outputImage;
        }

        /// <summary>
        /// Resamples the three-dimensional ITK image to the desired size.
        /// </summary>
        /// <param name="image">The three-dimensional ITK image.</param>
        /// <param name="desiredSize">The desired size.</param>
        /// <returns>
        /// The resampled three-dimensional ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        /// <exception cref="NotSupportedException">The dimension ({dimension}) of the provided image is not supported.</exception>
        public Image ResampleImage3D(Image image, uint desiredSize)
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

            var outputSize = new VectorUInt32()
            {
                desiredSize,
                desiredSize,
                desiredSize
            };

            var outputSpacing = new VectorDouble()
            {
                inputSpacing[0] * inputSize[0] / outputSize[0],
                inputSpacing[1] * inputSize[1] / outputSize[1],
                inputSpacing[2] * inputSize[2] / outputSize[2]
            };

            // https://itk.org/Wiki/ITK/Examples/ImageProcessing/ResampleImageFilter
            var filter = new ResampleImageFilter();
            filter.SetReferenceImage(image);
            filter.SetTransform(new ScaleTransform(image.GetDimension()));
            filter.SetInterpolator(InterpolatorEnum.sitkLinear);
            filter.SetSize(outputSize);
            filter.SetOutputSpacing(outputSpacing);
            return filter.Execute(image);
        }

        /// <summary>
        /// Gets the number of labels in the ITK image.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <returns>The number of labels in the ITK image.</returns>
        /// <exception cref="ArgumentNullException">image</exception>
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
            return filter.GetNumberOfLabels();
        }

        /// <summary>
        /// Converts the two-dimensional ITK image to a bitmap.
        /// </summary>
        /// <param name="image">The two-dimensional ITK image</param>
        /// <returns>
        /// The image as bitmap.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        /// <exception cref="NotSupportedException">The dimension ({dimension}) of the provided image is not supported.</exception>
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

        /// <summary>
        /// Convert the image to a specific format.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <returns>
        /// The rescaled ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
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
                RescaleIntensityImageFilter filterRS = new RescaleIntensityImageFilter();
                filterRS.SetOutputMinimum(0);
                filterRS.SetOutputMaximum(255);
                output = filterRS.Execute(image);
                filterRS.Dispose();
            }
            catch (Exception)
            {
                // ignore "RescaleIntensityImageFilter(000000F79D6CFC40):
                // Minimum output value cannot be greater than Maximum output value." exceptions
            }

            return output ?? image;
        }

        /// <summary>
        /// Applies the cast image filter.
        /// </summary>
        /// <param name="image">The ITK image.</param>
        /// <returns>
        /// The modified ITK image.
        /// </returns>
        /// <exception cref="ArgumentNullException">image</exception>
        private Image ApplyCastImageFilter(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // TODO: find mapping between itk.simple.PixelIDValueEnum and System.Drawing.Imaging.PixelFormat
            // see https://github.com/SimpleITK/SimpleITK/issues/582

            // Execute cast filter
            CastImageFilter filterC = new CastImageFilter();
            filterC.SetOutputPixelType(PixelIDValueEnum.sitkUInt8);
            Image output = filterC.Execute(image);
            filterC.Dispose();

            return output ?? image;
        }

        private IntPtr GetBuffer(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            PixelIDValueEnum imageType = PixelIDValueEnum.swigToEnum(image.GetPixelIDValue());

            if (imageType.swigValue == PixelIDValueEnum.sitkUInt8.swigValue)
            {
                return image.GetBufferAsUInt8();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkUInt16.swigValue)
            {
                return image.GetBufferAsUInt16();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkUInt32.swigValue)
            {
                return image.GetBufferAsUInt32();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkUInt64.swigValue)
            {
                throw new NotSupportedException($"The image type {imageType} is not supported.");
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkInt8.swigValue)
            {
                return image.GetBufferAsInt8();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkInt16.swigValue)
            {
                return image.GetBufferAsInt16();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkInt32.swigValue)
            {
                return image.GetBufferAsInt32();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkInt64.swigValue)
            {
                throw new NotSupportedException($"The image type {imageType} is not supported.");
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkFloat32.swigValue
                || imageType.swigValue == PixelIDValueEnum.sitkComplexFloat32.swigValue)
            {
                return image.GetBufferAsFloat();
            }
            else if (imageType.swigValue == PixelIDValueEnum.sitkFloat64.swigValue
                || imageType.swigValue == PixelIDValueEnum.sitkComplexFloat64.swigValue)
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
