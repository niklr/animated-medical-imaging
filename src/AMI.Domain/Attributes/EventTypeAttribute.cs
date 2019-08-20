using System;
using AMI.Domain.Enums.Auditing;

namespace AMI.Domain.Attributes
{
    /// <summary>
    /// An attribute used to annotate audit events.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field)]
    public class EventTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTypeAttribute"/> class.
        /// </summary>
        public EventTypeAttribute()
        {
            this.BaseEventType = BaseEventType.None;
        }

        /// <summary>
        /// Gets or sets the type of the base event.
        /// </summary>
        public BaseEventType BaseEventType { get; set; }
    }
}
