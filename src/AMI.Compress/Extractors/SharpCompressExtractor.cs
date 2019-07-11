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
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace AMI.Compress.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    /// <seealso cref="CompressibleExtractor" />
    public class SharpCompressExtractor : CompressibleExtractor
    {
        private readonly IFileSystemStrategy fileSystemStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressExtractor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <exception cref="ArgumentNullException">fileSystemStrategy</exception>
        public SharpCompressExtractor(IAppConfiguration configuration, IFileSystemStrategy fileSystemStrategy)
            : base(configuration)
        {
            this.fileSystemStrategy = fileSystemStrategy ?? throw new ArgumentNullException(nameof(fileSystemStrategy));
        }

        /// <inheritdoc/>
        public override async Task<IList<CompressedEntryModel>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct, int level = 0)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            if (level > 1)
            {
                throw new NotSupportedException("The archive contains too many levels.");
            }

            var fs = fileSystemStrategy.Create(sourcePath);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the provided source path.");
            }

            IList<CompressedEntryModel> entries = new List<CompressedEntryModel>();

            // TODO: add options as parameters
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
                    var sortedEntries = archive.Entries.Where(e => !e.IsDirectory).Sort(comparer).Take(MaxCompressibleEntries);
                    foreach (var entry in sortedEntries)
                    {
                        ct.ThrowIfCancellationRequested();

                        using (var ms = new MemoryStream())
                        {
                            entry.WriteTo(ms);
                            fs.File.WriteAllBytes(fs.Path.Combine(destinationPath, entry.Key), ms.ToArray());
                        }

                        entries.Add(EntryMapper.Map(entry));
                    }
                }
            }

            if (entries.Count == 1 && entries[0].Key.EndsWith(".tar"))
            {
                return await ExtractAsync(fs.Path.Combine(destinationPath, entries[0].Key), destinationPath, ct, ++level);
            }

            await Task.CompletedTask;

            return entries;
        }
    }
}
