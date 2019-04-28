using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMI.Compress.Mappers;
using AMI.Core.Configuration;
using AMI.Core.Models;
using AMI.Core.Readers;
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
                    var options = new ReaderOptions();

                    using (var file = File.OpenRead(path))
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
