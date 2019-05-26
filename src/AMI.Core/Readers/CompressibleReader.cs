using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Entities.Models;
using AMI.Core.Exceptions;

namespace AMI.Core.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public abstract class CompressibleReader : ICompressibleReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressibleReader" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public CompressibleReader(IAmiConfigurationManager configuration)
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
        /// Reads the specified compressed file asynchronous.
        /// </summary>
        /// <param name="path">The location of the compressed file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A list of compressed entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">path</exception>
        /// <exception cref="AmiException">
        /// The reading of the compressed file has been cancelled.
        /// or
        /// The compressed file could not be read.
        /// </exception>
        public Task<IList<CompressedEntry>> ReadAsync(string path, CancellationToken ct)
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
                throw new AmiException("The reading of the compressed file has been cancelled.", e);
            }
            catch (Exception e)
            {
                throw new AmiException("The compressed file could not be read.", e);
            }
        }

        /// <summary>
        /// Reads the specified compressed file asynchronous.
        /// </summary>
        /// <param name="path">The location of the compressed file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>
        /// A list of compressed entries.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// path
        /// or
        /// ct
        /// </exception>
        protected abstract Task<IList<CompressedEntry>> AbstractReadAsync(string path, CancellationToken ct);
    }
}
