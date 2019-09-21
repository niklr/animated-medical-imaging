using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about an event that occurred.
    /// An event is created for example when the status of a task changes.
    /// </summary>
    public class EventModel
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
        /// Gets or sets the API version used to render data.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the data associated with the event.
        /// </summary>
        public BaseEventDataModel Data { get; set; }
    }
}
