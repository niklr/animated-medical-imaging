﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Wrappers;
using AMI.Core.IO.Models;
using AMI.Core.IO.Writers;
using AMI.Core.Strategies;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using RNS.Framework.Tools;
using SharpCompress.Archives.Zip;

namespace AMI.Compress.Writers
{
    /// <summary>
    /// A writer to compress files.
    /// </summary>
    /// <seealso cref="IArchiveWriter" />
    public class SharpCompressWriter : IArchiveWriter
    {
        private readonly IFileSystemStrategy fileSystemStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressWriter"/> class.
        /// </summary>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <exception cref="ArgumentNullException">fileSystemStrategy</exception>
        public SharpCompressWriter(IFileSystemStrategy fileSystemStrategy)
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
        }

        /// <inheritdoc/>
        public async Task AddFilesAsync<T>(
            IEnumerable<T> items,
            Func<T, string> diskFilePathFunc,
            Func<T, string> entryNameFunc,
            IZipArchive archive,
            CancellationToken ct)
        {
            Ensure.ArgumentNotNull(items, nameof(items));
            Ensure.ArgumentNotNull(diskFilePathFunc, nameof(diskFilePathFunc));
            Ensure.ArgumentNotNull(entryNameFunc, nameof(entryNameFunc));
            Ensure.ArgumentNotNull(archive, nameof(archive));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            foreach (T item in items)
            {
                ct.ThrowIfCancellationRequested();

                string entryName = entryNameFunc(item);
                if (string.IsNullOrEmpty(entryName))
                {
                    continue;
                }

                string diskFilePath = diskFilePathFunc(item);
                if (string.IsNullOrEmpty(diskFilePath))
                {
                    continue;
                }

                if (archive.ContainsEntry(entryName))
                {
                    continue;
                }

                var fs = fileSystemStrategy.Create(diskFilePath);
                if (fs == null)
                {
                    throw new UnexpectedNullException("Filesystem could not be created based on the disk file path.");
                }

                Stream stream = fs.File.OpenRead(diskFilePath);
                archive.AddEntry(entryName, stream);
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public IZipArchive Create(CompressionType compressionType)
        {
            var archive = ZipArchive.Create();

            return new ZipArchiveWrapper(archive, compressionType);
        }

        /// <inheritdoc/>
        public void Write(Stream stream, IZipArchive archive)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (archive == null)
            {
                throw new ArgumentNullException(nameof(archive));
            }

            archive.Save(stream);
        }
    }
}
