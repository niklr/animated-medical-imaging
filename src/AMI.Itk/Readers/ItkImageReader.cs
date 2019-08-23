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
        public int Width
        {
            get { return Convert.ToInt32(size[0]); }
        }

        /// <inheritdoc/>
        public int Height
        {
            get { return Convert.ToInt32(size[1]); }
        }

        /// <inheritdoc/>
        public int Depth
        {
            get { return Convert.ToInt32(size[2]); }
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
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public System.Drawing.Bitmap ExtractPosition(AxisType axisType, int position, int? size)
        {
            Validate();

            int mappedPosition = Mapper == null ?
                position : Mapper.GetMappedPosition(axisType, position);

            using (Image image = itkUtil.ExtractPosition(Image, axisType, mappedPosition))
            {
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

        /// <inheritdoc/>
        public ulong GetLabelCount(AxisType axisType, int position)
        {
            Validate();

            int mappedPosition = Mapper == null ?
                position : Mapper.GetMappedPosition(axisType, position);

            using (Image image = itkUtil.ExtractPosition(Image, axisType, mappedPosition))
            {
                try
                {
                    return itkUtil.GetLabelCount(image);
                }
                catch (Exception)
                {
                    return 0;
                }
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
