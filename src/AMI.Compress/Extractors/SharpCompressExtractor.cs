using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Extractors;
using AMI.Core.Strategies;
using AMI.Domain.Exceptions;
using RNS.Framework.Comparers;
using RNS.Framework.Extensions.EnumerableExtensions;
using RNS.Framework.Tools;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace AMI.Compress.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    /// <seealso cref="ArchiveExtractor" />
    public class SharpCompressExtractor : ArchiveExtractor
    {
        private readonly IFileSystemStrategy fileSystemStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressExtractor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        public SharpCompressExtractor(IAppConfiguration configuration, IFileSystemStrategy fileSystemStrategy)
            : base(configuration)
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
        }

        /// <inheritdoc/>
        public override async Task<IList<ArchivedEntryModel>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct, int level = 0)
        {
            Ensure.ArgumentNotNullOrWhiteSpace(sourcePath, nameof(sourcePath));
            Ensure.ArgumentNotNullOrWhiteSpace(destinationPath, nameof(destinationPath));
            Ensure.ArgumentNotNull(ct, nameof(ct));

            if (level > 1)
            {
                throw new NotSupportedException("The archive contains too many levels.");
            }

            ct.ThrowIfCancellationRequested();

            var fs = fileSystemStrategy.Create(sourcePath);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the provided source path.");
            }

            IList<ArchivedEntryModel> entries = new List<ArchivedEntryModel>();

            var options = new ReaderOptions()
            {
                LeaveStreamOpen = false,
                LookForHeader = false
            };

            using (var file = fs.File.OpenRead(sourcePath))
            {
                using (var archive = ArchiveFactory.Open(file, options))
                using (var comparer = new GenericNaturalComparer<IArchiveEntry>(e => e.Key))
                {
                    var sortedEntries = archive.Entries
                        .Where(e => !e.IsDirectory)
                        .Sort(comparer)
                        .Take(MaxArchivedEntries > 0 ? MaxArchivedEntries : int.MaxValue);

                    if (MaxSizeKilobytes > 0 && sortedEntries.Sum(e => e.Size) > MaxSizeKilobytes * 1000)
                    {
                        throw new ArgumentException($"The file size exceeds the limit of {MaxSizeKilobytes} kilobytes.");
                    }

                    foreach (var entry in sortedEntries)
                    {
                        ct.ThrowIfCancellationRequested();

                        // extract all files to destination path (no sub-directories supported)
                        var filename = fs.Path.GetFileName(entry.Key);
                        if (string.IsNullOrWhiteSpace(filename))
                        {
                            continue;
                        }

                        using (var ms = new MemoryStream())
                        {
                            entry.WriteTo(ms);
                            fs.File.WriteAllBytes(fs.Path.Combine(destinationPath, filename), ms.ToArray());
                        }

                        var mappedEntry = EntryMapper.Map(entry);
                        mappedEntry.Key = filename;

                        entries.Add(mappedEntry);
                    }
                }
            }

            if (entries.Count == 1 && entries[0].Key.EndsWith(".tar"))
            {
                return await ExtractAsync(fs.Path.Combine(destinationPath, entries[0].Key), destinationPath, ct, ++level);
            }

            // Delete tarball after extraction
            if (level == 1 && sourcePath.EndsWith(".tar"))
            {
                fs.File.Delete(sourcePath);
            }

            await Task.CompletedTask;

            return entries;
        }
    }
}
