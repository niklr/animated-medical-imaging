using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using RNS.Framework.Tools;

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
        public ArchiveExtractor(IAppConfiguration configuration)
        {
            Ensure.ArgumentNotNull(configuration, nameof(configuration));

            MaxSizeKilobytes = configuration.Options.MaxSizeKilobytes;
            MaxArchivedEntries = configuration.Options.MaxArchivedEntries;
        }

        /// <summary>
        /// Gets the maximum size in kilobytes.
        /// </summary>
        public int MaxSizeKilobytes { get; private set; } = 0;

        /// <summary>
        /// Gets the maximum allowed amount of archived entries.
        /// </summary>
        public int MaxArchivedEntries { get; private set; } = 0;

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
        public abstract Task<IList<ArchivedEntryModel>> ExtractAsync(
            string sourcePath, string destinationPath, CancellationToken ct, int level = 0);
    }
}
