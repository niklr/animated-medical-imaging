using AMI.Core.Extractors;
using AMI.Core.Strategies;
using AMI.Itk.Readers;
using itk.simple;
using Microsoft.Extensions.Logging;

namespace AMI.Itk.Extractors
{
    /// <summary>
    /// An extractor for ITK images.
    /// </summary>
    /// <seealso cref="ImageExtractor{Image}" />
    public class ItkImageExtractor : ImageExtractor<Image>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageExtractor"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="reader">The image reader.</param>
        public ItkImageExtractor(ILoggerFactory loggerFactory, IFileSystemStrategy fileSystemStrategy, IItkImageReader reader)
            : base(loggerFactory, fileSystemStrategy, reader)
        {
        }
    }
}
