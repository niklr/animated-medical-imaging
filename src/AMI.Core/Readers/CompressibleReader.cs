using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configuration;
using AMI.Core.Exceptions;
using AMI.Core.Models;

namespace AMI.Core.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public abstract class CompressibleReader : ICompressibleReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressibleReader"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CompressibleReader(IAmiConfigurationManager configuration)
        {
            MaxCompressibleEntries = Convert.ToUInt32(configuration.MaxCompressedEntries);
        }

        /// <summary>
        /// Gets the maximum of compressible entries.
        /// </summary>
        public uint MaxCompressibleEntries { get; private set; } = uint.MinValue;

        /// <summary>
        /// Reads the specified compressed file asynchronous.
        /// </summary>
        /// <param name="path">The location of the compressed file.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A list of compressed entries.</returns>
        public Task<IList<CompressedEntry>> ReadAsync(string path, CancellationToken ct)
        {
            try
            {
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
        /// <returns>A list of compressed entries.</returns>
        protected abstract Task<IList<CompressedEntry>> AbstractReadAsync(string path, CancellationToken ct);
    }
}
