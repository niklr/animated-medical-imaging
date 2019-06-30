using System;
using System.Collections.Generic;
using System.IO;
using AMI.Domain.Enums;

namespace AMI.Core.IO.Models
{
    /// <summary>
    /// An interface representing a zip archive.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IZipArchive : IDisposable
    {
        /// <summary>
        /// Gets the compression type.
        /// </summary>
        CompressionType CompressionType { get; }

        /// <summary>
        /// Gets the total size of the files as uncompressed in the archive.
        /// </summary>
        long TotalSize { get; }

        /// <summary>
        /// Gets the total size of the files compressed in the archive.
        /// </summary>
        long TotalCompressedSize { get; }

        /// <summary>
        /// Adds the provided stream to the archive and closes the stream afterwards.
        /// </summary>
        /// <param name="key">The key representing the relative path of the file.</param>
        /// <param name="source">The source stream.</param>
        /// <returns>The zip entry.</returns>
        IZipEntry AddEntry(string key, Stream source);

        /// <summary>
        /// Determines whether the specified key is already part of the archive.
        /// </summary>
        /// <param name="key">The key representing the relative path of the file.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is already part of the archive; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsEntry(string key);

        /// <summary>
        /// Counts the number of entries contained in the archive.
        /// </summary>
        /// <returns>The number of entries contained in the archive</returns>
        int Count();

        /// <summary>
        /// Gets the entries contained in the archive.
        /// </summary>
        /// <returns>The entries contained in the archive</returns>
        IEnumerable<IZipEntry> Entries();

        /// <summary>
        /// Saves the archive to the specified stream.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        void Save(Stream stream);
    }
}
