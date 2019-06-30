using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Models;
using AMI.Domain.Enums;

namespace AMI.Core.IO.Writers
{
    /// <summary>
    /// A writer to compress files.
    /// </summary>
    public interface ICompressibleWriter
    {
        /// <summary>
        /// Adds the files asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="items">The items to compress.</param>
        /// <param name="diskFilePathFunc">The function to get the disk file path.</param>
        /// <param name="entryNameFunc">The function to get the entry name.</param>
        /// <param name="archive">The archive.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddFilesAsync<T>(
            IEnumerable<T> items,
            Func<T, string> diskFilePathFunc,
            Func<T, string> entryNameFunc,
            IZipArchive archive,
            CancellationToken ct);

        /// <summary>
        /// Creates a new zip archive.
        /// </summary>
        /// <param name="compressionType">The compression type.</param>
        /// <returns>The zip archive.</returns>
        IZipArchive Create(CompressionType compressionType);

        /// <summary>
        /// Writes the archive to the specified stream.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        /// <param name="archive">The archive.</param>
        void Write(Stream stream, IZipArchive archive);
    }
}
