using System;
using System.Collections.Generic;

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
        /// Gets or sets the tasks.
        /// </summary>
        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
