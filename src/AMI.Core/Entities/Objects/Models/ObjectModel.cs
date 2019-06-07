using System;

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
        /// Gets or sets the original filename.
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// Gets or sets the filesystem path.
        /// </summary>
        public string FilesystemPath { get; set; }
    }
}
