using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Enums;
using AMI.Core.Exceptions;
using AMI.Core.Mappers;
using AMI.Itk.Utils;
using itk.simple;

namespace AMI.Itk.Readers
{
    /// <summary>
    /// A reader for ITK images.
    /// </summary>
    /// <seealso cref="IItkImageReader" />
    public class ItkImageReader : IItkImageReader
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IItkUtil itkUtil;
        private VectorUInt32 size = new VectorUInt32();
        private IDictionary<AxisType, Image> axisImages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageReader"/> class.
        /// </summary>
        public ItkImageReader()
        {
            itkUtil = new ItkUtil();
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets the image width.
        /// </summary>
        public uint Width
        {
            get { return size[0]; }
        }

        /// <summary>
        /// Gets the image height.
        /// </summary>
        public uint Height
        {
            get { return size[1]; }
        }

        /// <summary>
        /// Gets the image depth.
        /// </summary>
        public uint Depth
        {
            get { return size[2]; }
        }

        /// <summary>
        /// Gets or sets the axis position mapper.
        /// </summary>
        public IAxisPositionMapper Mapper { get; set; }

        /// <summary>
        /// Initializes the reader asynchronous.
        /// </summary>
        /// <param name="path">The location of the image.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="AmiException">The ITK image could not be read.</exception>
        public async Task InitAsync(string path, CancellationToken ct)
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                Image = await itkUtil.ReadImageAsync(path, ct);
                if (Image == null)
                {
                    throw new AmiException("The ITK image could not be read.");
                }

                size = Image.GetSize();
                axisImages = new Dictionary<AxisType, Image>();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        /// <summary>
        /// Gets the recommended axis types.
        /// </summary>
        /// <returns>The recommended axis types.</returns>
        public ISet<AxisType> GetRecommendedAxisTypes()
        {
            var defaultAxisTypes = new HashSet<AxisType>() { AxisType.X, AxisType.Y, AxisType.Z };

            try
            {
                ISet<AxisType> axisTypes = new HashSet<AxisType>();

                var estimatedAreas = EstimateAreas();
                var maxEstimatedArea = estimatedAreas.Values.Max();

                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    double areaRatio = estimatedAreas[axisType] / maxEstimatedArea;
                    if (areaRatio > 0.05)
                    {
                        axisTypes.Add(axisType);
                    }
                }

                if (axisTypes.Count > 0)
                {
                    return axisTypes;
                }
                else
                {
                    return defaultAxisTypes;
                }
            }
            catch (Exception)
            {
                return defaultAxisTypes;
            }
        }

        /// <summary>
        /// Extracts the specified position as bitmap.
        /// </summary>
        /// <param name="axisType">Type of the axis.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The desired output size.</param>
        /// <returns>The extracted position as bitmap.</returns>
        public System.Drawing.Bitmap ExtractPosition(AxisType axisType, uint position, uint? size)
        {
            Validate();

            uint mappedPosition = Mapper == null ?
                position : Mapper.GetMappedPosition(axisType, position);

            Image image = itkUtil.ExtractPosition(Image, axisType, mappedPosition);

            if (axisImages.TryGetValue(axisType, out Image axisImage))
            {
                var filter = new AddImageFilter();
                axisImage = filter.Execute(axisImage, image);
                axisImages[axisType] = axisImage;
            }
            else
            {
                axisImages.Add(axisType, new Image(image));
            }

            if (size.HasValue && size.Value > 0)
            {
                return itkUtil.ToBitmap(itkUtil.ResampleImage2D(image, size.Value));
            }
            else
            {
                return itkUtil.ToBitmap(image);
            }
        }

        /// <summary>
        /// Gets the label count.
        /// </summary>
        /// <returns>The label count.</returns>
        public ulong GetLabelCount()
        {
            try
            {
                return itkUtil.GetLabelCount(Image);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private IDictionary<AxisType, double> EstimateAreas()
        {
            IDictionary<AxisType, double> areas = new Dictionary<AxisType, double>();

            try
            {
                foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
                {
                    areas.Add(axisType, 0);
                    if (axisImages.TryGetValue(axisType, out Image axisImage))
                    {
                        // WriteImage(axisImage, $@"C:\Temp\images\{axisType}.mha");
                        var spacing = axisImage.GetSpacing();
                        var areaOfOneVoxel = spacing[0] * spacing[1];

                        var filter = new StatisticsImageFilter();
                        filter.Execute(axisImage);
                        areas[axisType] += filter.GetSum() * areaOfOneVoxel;
                    }
                }

                return areas;
            }
            catch (Exception)
            {
                return areas;
            }
        }

        private void Validate()
        {
            if (Image == null)
            {
                throw new AmiException("The ITK image reader has not been initialized.");
            }
        }
    }
}
