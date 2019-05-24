﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configuration;
using AMI.Core.Models;
using AMI.Core.Readers;
using RNS.Framework.Comparers;
using RNS.Framework.Extensions.EnumerableExtensions;
using SharpCompress.Archives;
using SharpCompress.Readers;

namespace AMI.Compress.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public class SharpCompressReader : CompressibleReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressReader"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SharpCompressReader(IAmiConfigurationManager configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Reads the specified compressed file asynchronous.
        /// </summary>
        /// <param name="path">The location of the compressed file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of compressed entries.</returns>
        protected override async Task<IList<CompressedEntry>> AbstractReadAsync(string path, CancellationToken ct)
        {
            return await Task.Run(
                () =>
                {
                    IList<CompressedEntry> entries = new List<CompressedEntry>();

                    // TODO: add options as parameters
                    var options = new ReaderOptions()
                    {
                        LeaveStreamOpen = false,
                        LookForHeader = false
                    };

                    using (var file = File.OpenRead(path))
                    {
                        using (var archive = ArchiveFactory.Open(file, options))
                        using (var comparer = new GenericNaturalComparer<IArchiveEntry>(e => e.Key))
                        {
                            var sortedEntries = archive.Entries.Sort(comparer).Take(MaxCompressibleEntries);
                            foreach (var entry in sortedEntries)
                            {
                                entries.Add(EntryMapper.Map(entry));
                            }
                        }
                    }

                    return entries;
                }, ct);
        }
    }
}
