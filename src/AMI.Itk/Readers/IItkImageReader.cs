using AMI.Core.Readers;
using itk.simple;

namespace AMI.Itk.Readers
{
    /// <summary>
    /// A reader for ITK images.
    /// </summary>
    /// <seealso cref="IImageReader{Image}" />
    public interface IItkImageReader : IImageReader<Image>
    {
    }
}
