using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;

namespace AMI.Core.IO.Extractors
{
    /// <summary>
    /// An extractor for archived files.
    /// </summary>
    public abstract class ArchiveExtractor : IArchiveExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveExtractor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public ArchiveExtractor(IAppConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            MaxArchivedEntries = configuration.Options.MaxArchivedEntries;
        }

        /// <summary>
        /// Gets the maximum of archived entries.
        /// </summary>
        public int MaxArchivedEntries { get; private set; } = int.MinValue;

        /// <summary>
        /// Extracts the archived file asynchronous.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <param name="level">The recursion level.</param>
        /// <returns>
        /// A list of archived entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// sourcePath
        /// or
        /// destinationPath
        /// or
        /// ct
        /// </exception>
        public abstract Task<IList<ArchivedEntryModel>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct, int level = 0);
    }
}
