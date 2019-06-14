using System;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the task.
    /// </summary>
    public abstract class TaskModel
    {
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
        /// Gets or sets the status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the position in queue.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the progress (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the result.
        /// </summary>
        public string ResultId { get; set; }
    }
}
