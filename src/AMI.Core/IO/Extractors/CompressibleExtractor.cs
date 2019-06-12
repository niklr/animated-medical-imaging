using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;

namespace AMI.Core.IO.Extractors
{
    /// <summary>
    /// An extractor for compressed files.
    /// </summary>
    public abstract class CompressibleExtractor : ICompressibleExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressibleExtractor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public CompressibleExtractor(IAmiConfigurationManager configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            MaxCompressibleEntries = configuration.MaxCompressedEntries;
        }

        /// <summary>
        /// Gets the maximum of compressible entries.
        /// </summary>
        public int MaxCompressibleEntries { get; private set; } = int.MinValue;

        /// <summary>
        /// Extracts the compressed file asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A list of compressed entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// sourcePath
        /// or
        /// destinationPath
        /// or
        /// ct
        /// </exception>
        public abstract Task<IList<CompressedEntryModel>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct);
    }
}
