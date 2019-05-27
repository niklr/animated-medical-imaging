using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using AMI.Core.Factories;
using AMI.Domain.Exceptions;

namespace AMI.Core.Strategies
{
    /// <summary>
    /// A strategy for file system purposes.
    /// </summary>
    /// <seealso cref="IFileSystemStrategy" />
    public class FileSystemStrategy : IFileSystemStrategy
    {
        private readonly IList<IFileSystemFactory> factories = new List<IFileSystemFactory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemStrategy"/> class.
        /// </summary>
        public FileSystemStrategy()
        {
            factories.Add(new FileSystemFactory());
        }

        /// <summary>
        /// Creates an instance of a file system based on the provided path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The file system instance.
        /// </returns>
        /// <exception cref="AmiException">The provided path is not supported.</exception>
        public IFileSystem Create(string path)
        {
            var factory = this.factories.FirstOrDefault(f => f.AppliesTo(path));

            if (factory == null)
            {
                throw new AmiException("The provided path is not supported.");
            }

            return factory.Create();
        }
    }
}
