using System;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing an object.
    /// </summary>
    public class ObjectModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        public ObjectModel()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

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
        public DataType DataType { get; set; } = DataType.Unknown;

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public FileFormat FileFormat { get; set; } = FileFormat.Unknown;

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
