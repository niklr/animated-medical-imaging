﻿namespace AMI.Core.IO.Models
{
    /// <summary>
    /// An interface representing a zip archive entry.
    /// </summary>
    public interface IZipEntry
    {
        /// <summary>
        /// Gets the key representing the relative path of the file.
        /// </summary>
        /// <example>
        /// /Dir1/Dir2/MyFile.tmp
        /// </example>
        string Key { get; }

        /// <summary>
        /// Gets the size of the compressed file.
        /// </summary>
        long CompressedSize { get; }

        /// <summary>
        /// Gets the size of the uncompressed file.
        /// </summary>
        long Size { get; }
    }
}
