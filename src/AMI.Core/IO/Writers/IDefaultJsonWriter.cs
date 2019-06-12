using System;
using System.Threading.Tasks;

namespace AMI.Core.IO.Writers
{
    /// <summary>
    /// A writer for JSON data.
    /// </summary>
    public interface IDefaultJsonWriter
    {
        /// <summary>
        /// Writes the JSON data asynchronous.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="name">The filename without extension.</param>
        /// <param name="content">The content to be written.</param>
        /// <param name="filenameCallback">The filename callback.</param>
        /// <returns>The filename with extension.</returns>
        Task<string> WriteAsync(string destinationPath, string name, object content, Action<string> filenameCallback = null);
    }
}
