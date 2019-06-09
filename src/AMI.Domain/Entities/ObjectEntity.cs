using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing an object.
    /// </summary>
    public class ObjectEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public int FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the source path (directory, file, url, etc.).
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the uncompressed filesystem path (directory).
        /// </summary>
        public string UncompressedFilesystemPath { get; set; }
    }
}
