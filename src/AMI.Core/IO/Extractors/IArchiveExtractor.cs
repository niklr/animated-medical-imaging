using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;

namespace AMI.Core.IO.Extractors
{
    /// <summary>
    /// An extractor for archived files.
    /// </summary>
    public interface IArchiveExtractor
    {
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
        Task<IList<ArchivedEntryModel>> ExtractAsync(string sourcePath, string destinationPath, CancellationToken ct, int level = 0);
    }
}
