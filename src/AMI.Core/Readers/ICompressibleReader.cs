﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;

namespace AMI.Core.Readers
{
    /// <summary>
    /// A reader for compressed files.
    /// </summary>
    public interface ICompressibleReader
    {
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
        Task<IList<CompressedEntryModel>> ReadAsync(string path, CancellationToken ct);
    }
}
