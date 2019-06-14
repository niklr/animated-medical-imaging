using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a task.
    /// </summary>
    public class TaskEntity
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
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the message describing the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the position in queue.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the progress (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        public int CommandType { get; set; }

        /// <summary>
        /// Gets or sets the serialized command.
        /// </summary>
        public string CommandSerialized { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        public ObjectEntity Object { get; set; }

        /// <summary>
        /// Gets or sets the result identifier.
        /// </summary>
        public Guid ResultId { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public ResultEntity Result { get; set; }
    }
}
