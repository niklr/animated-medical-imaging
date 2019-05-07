using AMI.Core.Extractors;
using AMI.Core.Strategies;
using AMI.Itk.Factories;
using AMI.Itk.Readers;
using itk.simple;
using Microsoft.Extensions.Logging;

namespace AMI.Itk.Extractors
{
    /// <summary>
    /// An extractor for ITK images.
    /// </summary>
    /// <seealso cref="ImageExtractor{IItkImageReader, Image}" />
    public class ItkImageExtractor : ImageExtractor<IItkImageReader, Image>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageExtractor"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="readerFactory">The image reader factory.</param>
        public ItkImageExtractor(ILoggerFactory loggerFactory, IFileSystemStrategy fileSystemStrategy, IItkImageReaderFactory readerFactory)
            : base(loggerFactory, fileSystemStrategy, readerFactory)
        {
        }
    }
}
