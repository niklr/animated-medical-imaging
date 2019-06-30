using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMI.Core.IO.Models;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using SharpCompress.Archives.Zip;
using SharpCompress.Writers;

namespace AMI.Compress.Wrappers
{
    /// <summary>
    /// A wrapper for zip archives.
    /// </summary>
    /// <seealso cref="IZipArchive" />
    public class ZipArchiveWrapper : IZipArchive
    {
        private readonly ZipArchive archive;
        private readonly WriterOptions writerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiveWrapper"/> class.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="compressionType">The compression type.</param>
        /// <exception cref="ArgumentNullException">archive</exception>
        public ZipArchiveWrapper(ZipArchive archive, CompressionType compressionType)
        {
            this.archive = archive ?? throw new ArgumentNullException(nameof(archive));
            CompressionType = compressionType;

            switch (compressionType)
            {
                case CompressionType.None:
                    writerOptions = new WriterOptions(SharpCompress.Common.CompressionType.None);
                    break;
                case CompressionType.Deflate:
                    writerOptions = new WriterOptions(SharpCompress.Common.CompressionType.Deflate);
                    break;
                default:
                    CompressionType = CompressionType.Deflate;
                    writerOptions = new WriterOptions(SharpCompress.Common.CompressionType.Deflate);
                    break;
            }
        }

        /// <inheritdoc/>
        public CompressionType CompressionType { get; }

        /// <inheritdoc/>
        public long TotalSize
        {
            get
            {
                return archive.TotalUncompressSize;
            }
        }

        /// <inheritdoc/>
        public long TotalCompressedSize
        {
            get
            {
                return archive.TotalSize;
            }
        }

        /// <inheritdoc/>
        public IZipEntry AddEntry(string key, Stream source)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var entry = archive.AddEntry(key, source, true);
            if (entry == null)
            {
                throw new UnexpectedNullException("The entry added to the archive was null.");
            }

            return new ZipEntryWrapper(entry);
        }

        /// <inheritdoc/>
        public bool ContainsEntry(string key)
        {
            return archive.Entries.Any(e => e.Key == key);
        }

        /// <inheritdoc/>
        public int Count()
        {
            return archive.Entries.Count;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            archive.Dispose();
        }

        /// <inheritdoc/>
        public IEnumerable<IZipEntry> Entries()
        {
            return archive.Entries.Select(e => new ZipEntryWrapper(e));
        }

        /// <inheritdoc/>
        public void Save(Stream stream)
        {
            archive.SaveTo(stream, writerOptions);
        }
    }
}
