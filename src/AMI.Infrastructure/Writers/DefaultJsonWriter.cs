using System;
using System.Text;
using System.Threading.Tasks;
using AMI.Core.Serializers;
using AMI.Core.Strategies;
using AMI.Core.Writers;
using AMI.Domain.Exceptions;

namespace AMI.Infrastructure.Writers
{
    /// <summary>
    /// A writer for JSON data.
    /// </summary>
    /// <seealso cref="IDefaultJsonWriter" />
    public class DefaultJsonWriter : IDefaultJsonWriter
    {
        private readonly IDefaultJsonSerializer serializer;
        private readonly IFileSystemStrategy fileSystemStrategy;
        private readonly string extension = ".json";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonWriter"/> class.
        /// </summary>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <exception cref="ArgumentNullException">
        /// serializer
        /// or
        /// fileSystemStrategy
        /// </exception>
        public DefaultJsonWriter(IDefaultJsonSerializer serializer, IFileSystemStrategy fileSystemStrategy)
        {
            this.serializer = serializer;
            if (this.serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            this.fileSystemStrategy = fileSystemStrategy;
            if (this.fileSystemStrategy == null)
            {
                throw new ArgumentNullException(nameof(fileSystemStrategy));
            }
        }

        /// <summary>
        /// Writes the JSON data asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="name">The filename without extension.</param>
        /// <param name="content">The content to be written.</param>
        /// <param name="filenameCallback">The filename callback.</param>
        /// <returns>The filename with extension.</returns>
        public async Task<string> WriteAsync(string destinationPath, string name, object content, Action<string> filenameCallback = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var destinationFileSystem = fileSystemStrategy.Create(destinationPath);
                    destinationFileSystem.Directory.CreateDirectory(destinationPath);

                    string filename = $"{name}{extension}";

                    filenameCallback?.Invoke(filename);

                    string destinationFullPath = destinationFileSystem.Path.Combine(destinationPath, filename);
                    destinationFileSystem.File.WriteAllBytes(destinationFullPath, Encoding.UTF8.GetBytes(serializer.Serialize(content)));

                    return filename;
                }
                catch (Exception e)
                {
                    throw new AmiException("The JSON could not be written.", e);
                }
            });
        }
    }
}
