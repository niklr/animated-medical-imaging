using AMI.Core.Entities.Models;
using AMI.Domain.Enums;
using MediatR;

namespace AMI.Core.Entities.Events.Commands.Create
{
    /// <summary>
    /// A command containing information needed to create an event.
    /// </summary>
    public class CreateEventCommand : IRequest<EventModel>
    {
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public object Event { get; set; }
    }
}
