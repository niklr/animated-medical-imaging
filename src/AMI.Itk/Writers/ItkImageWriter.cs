using System.Threading;
using System.Threading.Tasks;
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
        private readonly IItkUtil itkUtil;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItkImageWriter"/> class.
        /// </summary>
        public ItkImageWriter()
        {
            itkUtil = new ItkUtil();
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
