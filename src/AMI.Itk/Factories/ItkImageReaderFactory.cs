using AMI.Core.Factories;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageReaderFactory"/> class.
        /// </summary>
        public ItkImageReaderFactory()
            : base()
        {
        }

        /// <inheritdoc/>
        public override IItkImageReader Create()
        {
            return new ItkImageReader();
        }
    }
}
