using System;
using System.IO;
using System.IO.Abstractions;

namespace AMI.Core.Extensions.FileSystemExtensions
{
    /// <summary>
    /// Extensions related to file systems.
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Builds the absolute path.
        /// </summary>
        /// <param name="fs">The file system.</param>
        /// <param name="path">The partial path.</param>
        /// <returns>
        /// The absolute path.
        /// </returns>
        /// <exception cref="ArgumentNullException">fs</exception>
        public static string BuildAbsolutePath(this IFileSystem fs, string path)
        {
            if (fs == null)
            {
                throw new ArgumentNullException(nameof(fs));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            else if (fs.Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return fs.Path.GetFullPath(path);
            }
        }

        /// <summary>
        /// Determines whether the specified path is a directory.
        /// </summary>
        /// <param name="fs">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is a directory; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirectory(this IFileSystem fs, string path)
        {
            try
            {
                return fs.File.GetAttributes(@"c:\Temp").HasFlag(FileAttributes.Directory);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
