﻿using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configuration;
using AMI.Core.Extractors;
using AMI.Core.Models;
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

        /// <summary>
        /// Extracts the compressed file asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of compressed entries.</returns>
        public async Task<IList<CompressedEntry>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct)
        {
            return await Task.Run(
                () =>
                {
                    IList<CompressedEntry> entries = new List<CompressedEntry>();

                    // TODO: add options as parameters
                    var options = new ReaderOptions();

                    using (var file = File.OpenRead(sourcePath))
                    {
                        using (var reader = ReaderFactory.Open(file, options))
                        {
                            int count = 0;
                            while (reader.MoveToNextEntry())
                            {
                                ct.ThrowIfCancellationRequested();

                                if (!MaxCompressibleEntries.Equals(uint.MinValue) && MaxCompressibleEntries <= count)
                                {
                                    break;
                                }

                                if (!reader.Entry.IsDirectory)
                                {
                                    // TODO: add options as parameters
                                    var extractionOptions = new ExtractionOptions()
                                    {
                                        ExtractFullPath = true,
                                        Overwrite = true
                                    };

                                    reader.WriteEntryToDirectory(destinationPath, extractionOptions);
                                }

                                entries.Add(EntryMapper.Map(reader.Entry));

                                count++;
                            }
                        }
                    }

                    return entries;
                }, ct);
        }
    }
}
