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
        /// <returns>The absolute path.</returns>
        public static string BuildAbsolutePath(this IFileSystem fs, string path)
        {
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
        /// Clears the directory.
        /// </summary>
        /// <param name="fs">The file system.</param>
        /// <param name="path">The path.</param>
        public static void ClearDirectory(this IFileSystem fs, string path)
        {
            IDirectoryInfo di = fs.Directory.CreateDirectory(path);
            foreach (IFileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
