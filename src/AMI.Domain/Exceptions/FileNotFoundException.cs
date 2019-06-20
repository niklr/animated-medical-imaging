using System;

namespace AMI.Domain.Exceptions
{
    /// <summary>
    /// This exception is thrown when a file was not found.
    /// </summary>
    /// <seealso cref="Exception" />
    public class FileNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileNotFoundException"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        public FileNotFoundException(string path)
            : base($"File \"{path}\" was not found.")
        {
        }
    }
}