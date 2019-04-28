﻿using System;

namespace AMI.Core.Models
{
    /// <summary>
    /// A model containing information about the compressed entry.
    /// </summary>
    public class CompressedEntry
    {
        /// <summary>
        /// Gets or sets the archived time.
        /// </summary>
        public DateTime? ArchivedTime { get; set; }

        /// <summary>
        /// Gets or sets the compressed size.
        /// </summary>
        public long CompressedSize { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this compressed entry is a directory.
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this compressed entry is encrypted.
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// Gets or sets the last access date.
        /// </summary>
        public DateTime? LastAccessedTime { get; set; }

        /// <summary>
        /// Gets or sets the last modification date.
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public long Size { get; set; }
    }
}
