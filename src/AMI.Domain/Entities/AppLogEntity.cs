﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a log entry generated by the application.
    /// </summary>
    public class AppLogEntity
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the source context.
        /// </summary>
        public string SourceContext { get; set; }
    }
}
