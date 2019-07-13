using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Itk.Readers;
using AMI.Itk.Utils;

namespace AMI.Itk.Writers
{
    /// <summary>
    /// A writer for ITK images.
    /// </summary>
    /// <seealso cref="IItkImageWriter" />
    public class ItkImageWriter : IItkImageWriter
    {
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IFileExtensionMapper fileExtensionMapper;
        private readonly IItkUtil itkUtil;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageWriter"/> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="fileExtensionMapper">The file extension mapper.</param>
        public ItkImageWriter(IFileSystemStrategy fileSystemStrategy, IFileExtensionMapper fileExtensionMapper)
            : base()
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.fileExtensionMapper = fileExtensionMapper ?? throw new ArgumentNullException(nameof(fileExtensionMapper));

            itkUtil = new ItkUtil(fileSystemStrategy, fileExtensionMapper);
        }

        /// <summary>
        /// Writes the images asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="reader">The image reader.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task WriteAsync(string destinationPath, string filename, IItkImageReader reader, CancellationToken ct)
        {
            await itkUtil.WriteImageAsync(reader.Image, destinationPath, filename, ct);
        }
    }
}
