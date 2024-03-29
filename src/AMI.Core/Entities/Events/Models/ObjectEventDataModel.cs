﻿namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the event.
    /// </summary>
    public class ObjectEventDataModel : BaseEventDataModel
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        public ObjectModel Object { get; set; }
    }
}