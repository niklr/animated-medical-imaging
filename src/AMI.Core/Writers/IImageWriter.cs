using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Readers;

namespace AMI.Core.Writers
{
    /// <summary>
    /// A writer for imaging purposes.
    /// </summary>
    /// <typeparam name="T1">The type of the reader.</typeparam>
    /// <typeparam name="T2">The type of the image.</typeparam>
    public interface IImageWriter<T1, T2>
        where T1 : IImageReader<T2>
    {
        /// <summary>
        /// Writes the images asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="reader">The image reader.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteAsync(string destinationPath, string filename, T1 reader, CancellationToken ct);
    }
}
