using System;
using AMI.Core.IO.Models;
using SharpCompress.Archives;

namespace AMI.Compress.Wrappers
{
    /// <summary>
    /// A wrapper for zip entries.
    /// </summary>
    /// <seealso cref="IZipEntry" />
    public class ZipEntryWrapper : IZipEntry
    {
        private readonly IArchiveEntry entry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipEntryWrapper"/> class.
        /// </summary>
        /// <param name="entry">The entry to wrap.</param>
        /// <exception cref="ArgumentNullException">entry</exception>
        public ZipEntryWrapper(IArchiveEntry entry)
        {
            this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        /// <inheritdoc/>
        public string Key
        {
            get
            {
                return entry.Key;
            }
        }
    }
}
