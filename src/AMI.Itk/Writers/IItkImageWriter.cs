using AMI.Core.Writers;
using AMI.Itk.Readers;
using itk.simple;

namespace AMI.Itk.Writers
{
    /// <summary>
    /// A writer for ITK images.
    /// </summary>
    /// <seealso cref="IImageWriter{IItkImageReader, Image}" />
    public interface IItkImageWriter : IImageWriter<IItkImageReader, Image>
    {
    }
}
