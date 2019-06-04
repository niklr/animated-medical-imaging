using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model representing an object result.
    /// </summary>
    public class ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectResult"/> class.
        /// </summary>
        public ObjectResult()
        {
            Status = new ObjectStatus();
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
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the filesystem path.
        /// </summary>
        public string FilesystemPath { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public ObjectStatus Status { get; set; }
    }
}
