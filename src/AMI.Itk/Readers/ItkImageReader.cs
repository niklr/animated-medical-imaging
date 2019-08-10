using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using AMI.Itk.Utils;
using itk.simple;
using RNS.Framework.Tools;

namespace AMI.Itk.Readers
{
    /// <summary>
    /// A reader for ITK images.
    /// </summary>
    /// <seealso cref="IItkImageReader" />
    public class ItkImageReader : IItkImageReader
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IFileExtensionMapper fileExtensionMapper;
        private readonly IItkUtil itkUtil;
        private VectorUInt32 size = new VectorUInt32();
        private IDictionary<AxisType, Image> axisImages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageReader"/> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="fileExtensionMapper">The file extension mapper.</param>
        public ItkImageReader(IFileSystemStrategy fileSystemStrategy, IFileExtensionMapper fileExtensionMapper)
            : base()
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.fileExtensionMapper = fileExtensionMapper ?? throw new ArgumentNullException(nameof(fileExtensionMapper));

            itkUtil = new ItkUtil(fileSystemStrategy, fileExtensionMapper);
        }

        /// <inheritdoc/>
        public Image Image { get; private set; }

        /// <inheritdoc/>
        public uint Width
        {
            get { return size[0]; }
        }

        /// <inheritdoc/>
        public uint Height
        {
            get { return size[1]; }
        }

        /// <inheritdoc/>
        public uint Depth
        {
            get { return size[2]; }
        }

        /// <inheritdoc/>
        public IAxisPositionMapper Mapper { get; set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }

            if (axisImages != null)
            {
                foreach (var axisImage in axisImages)
                {
                    axisImage.Value.Dispose();
                }

                axisImages = new Dictionary<AxisType, Image>();
            }
        }

        /// <inheritdoc/>
        public async Task InitAsync(string path, CancellationToken ct)
        {
            Ensure.ArgumentNotNull(path, nameof(path));
            Ensure.ArgumentNotNull(ct, nameof(ct));

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

        /// <inheritdoc/>
        public ISet<AxisType> GetRecommendedAxisTypes()
        {
            Validate();

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

        /// <inheritdoc/>
        public System.Drawing.Bitmap ExtractPosition(AxisType axisType, uint position, uint? size)
        {
            Validate();

            uint mappedPosition = Mapper == null ?
                position : Mapper.GetMappedPosition(axisType, position);

            using (Image image = itkUtil.ExtractPosition(Image, axisType, mappedPosition))
            {
                if (axisImages.TryGetValue(axisType, out Image axisImage))
                {
                    var filter = new AddImageFilter();
                    axisImage = filter.Execute(axisImage, image);
                    axisImages[axisType] = axisImage;
                    filter.Dispose();
                }
                else
                {
                    axisImages.Add(axisType, new Image(image));
                }

                if (size.HasValue && size.Value > 0)
                {
                    using (var resampledImage = itkUtil.ResampleImage2D(image, size.Value))
                    {
                        return itkUtil.ToBitmap(resampledImage);
                    }
                }
                else
                {
                    return itkUtil.ToBitmap(image);
                }
            }
        }

        /// <inheritdoc/>
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
                        filter.Dispose();
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
