using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;

namespace AMI.Core.IO.Readers
{
    /// <summary>
    /// A reader for archived files.
    /// </summary>
    public interface IArchiveReader
    {
        /// <summary>
        /// Determines whether the specified path is an archive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is an archive; otherwise, <c>false</c>.
        /// </returns>
        bool IsArchive(string path);

        /// <summary>
        /// Reads the specified archived file asynchronous.
        /// </summary>
        /// <param name="path">The location of the archived file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A list of archived entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">path</exception>
        /// <exception cref="AmiException">
        /// The reading of the archived file has been cancelled.
        /// or
        /// The archived file could not be read.
        /// </exception>
        Task<IList<ArchivedEntryModel>> ReadAsync(string path, CancellationToken ct);
    }
}
