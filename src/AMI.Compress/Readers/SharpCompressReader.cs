using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.IO.Readers;
using AMI.Core.Mappers;
using AMI.Core.Strategies;
using AMI.Domain.Exceptions;
using RNS.Framework.Comparers;
using RNS.Framework.Extensions.EnumerableExtensions;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace AMI.Compress.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public class SharpCompressReader : ArchiveReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressReader"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="fileSystemStrategy">The file system strategy.</param>
        /// <param name="fileExtensionMapper">The file extension mapper.</param>
        public SharpCompressReader(IAppConfiguration configuration, IFileSystemStrategy fileSystemStrategy, IFileExtensionMapper fileExtensionMapper)
            : base(configuration, fileSystemStrategy, fileExtensionMapper)
        {
        }

        /// <inheritdoc/>
        protected override async Task<IList<ArchivedEntryModel>> AbstractReadAsync(string path, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (ct == null)
            {
                throw new ArgumentNullException(nameof(ct));
            }

            var fs = FileSystemStrategy.Create(path);
            if (fs == null)
            {
                throw new UnexpectedNullException("Filesystem could not be created based on the provided path.");
            }

            IList<ArchivedEntryModel> entries = new List<ArchivedEntryModel>();

            var options = new ReaderOptions()
            {
                LeaveStreamOpen = false,
                LookForHeader = false
            };

            using (var file = fs.File.OpenRead(path))
            {
                using (var archive = ArchiveFactory.Open(file, options))
                using (var comparer = new GenericNaturalComparer<IArchiveEntry>(e => e.Key))
                {
                    var sortedEntries = archive.Entries.Sort(comparer).Take(MaxArchivedEntries);
                    foreach (var entry in sortedEntries)
                    {
                        ct.ThrowIfCancellationRequested();
                        entries.Add(EntryMapper.Map(entry));
                    }
                }
            }

            await Task.CompletedTask;

            return entries;
        }
    }
}
