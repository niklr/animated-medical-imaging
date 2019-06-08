using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing an object.
    /// </summary>
    public class ObjectVersion
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
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the filesystem path.
        /// </summary>
        public string FsPath { get; set; }
    }
}
