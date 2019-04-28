using System.IO.Abstractions;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for file systems.
    /// </summary>
    public interface IFileSystemFactory
    {
        /// <summary>
        /// Determines whether this factory can provide a file system for the provided path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///     <c>true</c> if the provided path is supported by this factory; otherwise, <c>false</c>.
        /// </returns>
        bool AppliesTo(string path);

        /// <summary>
        /// Creates a new file system instance.
        /// </summary>
        /// <returns>The file system instance.</returns>
        IFileSystem Create();
    }
}
