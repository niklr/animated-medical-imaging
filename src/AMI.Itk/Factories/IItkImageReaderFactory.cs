using AMI.Core.Factories;
using AMI.Itk.Readers;
using itk.simple;

namespace AMI.Itk.Factories
{
    /// <summary>
    /// A factory for the ITK image reader.
    /// </summary>
    /// <seealso cref="IImageReaderFactory{IItkImageReader, Image}" />
    public interface IItkImageReaderFactory : IImageReaderFactory<IItkImageReader, Image>
    {
        /// <summary>
        /// Creates a new instance of the ITK image reader.
        /// </summary>
        /// <returns>The ITK image reader.</returns>
        new IItkImageReader Create();
    }
}
