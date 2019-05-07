using AMI.Core.Readers;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for an image reader.
    /// </summary>
    /// <typeparam name="T1">The type of the image reader.</typeparam>
    /// <typeparam name="T2">The type of the image.</typeparam>
    public interface IImageReaderFactory<T1, T2>
        where T1 : IImageReader<T2>
    {
        /// <summary>
        /// Creates a new instance of the image reader.
        /// </summary>
        /// <returns>The image reader.</returns>
        T1 Create();
    }
}
