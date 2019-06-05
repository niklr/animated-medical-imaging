using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configuration;
using AMI.Core.Entities.Models;
using AMI.Core.Extractors;
using RNS.Framework.Comparers;
using RNS.Framework.Extensions.EnumerableExtensions;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace AMI.Compress.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    /// <seealso cref="CompressibleExtractor" />
    public class SharpCompressExtractor : CompressibleExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpCompressExtractor"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SharpCompressExtractor(IAmiConfigurationManager configuration)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public override async Task<IList<CompressedEntry>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct)
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

                    using (var file = File.OpenRead(sourcePath))
                    {
                        using (var archive = ArchiveFactory.Open(file, options))
                        using (var comparer = new GenericNaturalComparer<IArchiveEntry>(e => e.Key))
                        {
                            var sortedEntries = archive.Entries.Sort(comparer).Take(MaxCompressibleEntries);
                            foreach (var entry in sortedEntries)
                            {
                                if (!entry.IsDirectory)
                                {
                                    // TODO: add options as parameters
                                    var extractionOptions = new ExtractionOptions()
                                    {
                                        ExtractFullPath = false,
                                        Overwrite = false
                                    };

                                    entry.WriteToDirectory(destinationPath, extractionOptions);
                                    entries.Add(EntryMapper.Map(entry));
                                }
                            }
                        }
                    }

                    return entries;
                }, ct);
        }
    }
}
