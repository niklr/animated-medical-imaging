using System.IO.Abstractions;

namespace AMI.Core.Factories
{
    /// <summary>
    /// A factory for file systems.
    /// </summary>
    /// <seealso cref="IFileSystemFactory" />
    public class FileSystemFactory : IFileSystemFactory
    {
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemFactory"/> class.
        /// </summary>
        public FileSystemFactory()
        {
            fileSystem = new FileSystem();
        }

        /// <summary>
        /// Determines whether this factory can provide a file system for the provided path.
        /// </summary>
        /// <param name="path">The path used to determine the file system.</param>
        /// <returns>
        ///     <c>true</c> if the provided path is supported by this factory; otherwise, <c>false</c>.
        /// </returns>
        public bool AppliesTo(string path)
        {
            return true;
        }

        /// <summary>
        /// Creates a new file system instance.
        /// </summary>
        /// <returns>The file system instance.</returns>
        public IFileSystem Create()
        {
            return fileSystem;
        }
    }
}
