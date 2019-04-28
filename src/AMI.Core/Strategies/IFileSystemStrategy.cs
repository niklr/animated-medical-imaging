using System.IO.Abstractions;

namespace AMI.Core.Strategies
{
    /// <summary>
    /// A strategy for file system purposes.
    /// </summary>
    public interface IFileSystemStrategy
    {
        /// <summary>
        /// Creates an instance of a file system based on the provided path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The file system instance.</returns>
        IFileSystem Create(string path);
    }
}
