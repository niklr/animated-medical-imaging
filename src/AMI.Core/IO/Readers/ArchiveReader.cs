using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;

namespace AMI.Core.IO.Readers
{
    /// <summary>
    /// A reader for archived files.
    /// </summary>
    public abstract class ArchiveReader : IArchiveReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveReader" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public ArchiveReader(IAppConfiguration configuration)
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
        public Task<IList<ArchivedEntryModel>> ReadAsync(string path, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    throw new ArgumentNullException(nameof(path));
                }

                return AbstractReadAsync(path, ct);
            }
            catch (OperationCanceledException e)
            {
                throw new AmiException("The reading of the archived file has been cancelled.", e);
            }
            catch (Exception e)
            {
                throw new AmiException("The archived file could not be read.", e);
            }
        }

        /// <summary>
        /// Reads the specified archived file asynchronous.
        /// </summary>
        /// <param name="path">The location of the archived file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A list of archived entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// path
        /// or
        /// ct
        /// </exception>
        protected abstract Task<IList<ArchivedEntryModel>> AbstractReadAsync(string path, CancellationToken ct);
    }
}
