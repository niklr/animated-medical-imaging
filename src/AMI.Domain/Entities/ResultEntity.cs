using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a result.
    /// </summary>
    public class ResultEntity
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
        /// Gets or sets the application version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the JSON filename.
        /// </summary>
        public string JsonFilename { get; set; }

        /// <summary>
        /// Gets or sets the type of the result.
        /// </summary>
        public int ResultType { get; set; }

        /// <summary>
        /// Gets or sets the serialized result.
        /// </summary>
        public string ResultSerialized { get; set; }

        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        public TaskEntity Task { get; set; }
    }
}
