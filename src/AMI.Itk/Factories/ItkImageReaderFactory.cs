using System;
using AMI.Core.Factories;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Itk.Readers;
using itk.simple;

namespace AMI.Itk.Factories
{
    /// <summary>
    /// A factory for the ITK image reader.
    /// </summary>
    /// <seealso cref="ImageReaderFactory{IItkImageReader, Image}" />
    /// <seealso cref="IItkImageReaderFactory" />
    public class ItkImageReaderFactory : ImageReaderFactory<IItkImageReader, Image>, IItkImageReaderFactory
    {
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly IFileExtensionMapper fileExtensionMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageReaderFactory"/> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="fileExtensionMapper">The file extension mapper.</param>
        public ItkImageReaderFactory(IFileSystemStrategy fileSystemStrategy, IFileExtensionMapper fileExtensionMapper)
            : base()
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
            this.fileExtensionMapper = fileExtensionMapper ?? throw new ArgumentNullException(nameof(fileExtensionMapper));
        }

        /// <inheritdoc/>
        public override IItkImageReader Create()
        {
            return new ItkImageReader(fileSystemStrategy, fileExtensionMapper);
        }
    }
}
