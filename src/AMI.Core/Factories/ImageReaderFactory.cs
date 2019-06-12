using AMI.Core.IO.Readers;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for an image reader.
    /// </summary>
    /// <typeparam name="T1">The type of the image reader.</typeparam>
    /// <typeparam name="T2">The type of the image.</typeparam>
    public abstract class ImageReaderFactory<T1, T2> : IImageReaderFactory<T1, T2>
        where T1 : IImageReader<T2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageReaderFactory{T1, T2}"/> class.
        /// </summary>
        public ImageReaderFactory()
        {
        }

        /// <summary>
        /// Creates a new instance of the image reader.
        /// </summary>
        /// <returns>The image reader.</returns>
        public abstract T1 Create();
    }
}
