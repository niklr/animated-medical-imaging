using AMI.Core.Entities.Models;
using MediatR;
using XDASv2Net.Model;

namespace AMI.Core.Entities.AuditEvents.Commands.Create
{
    /// <summary>
    /// A command containing information needed to create a task.
    /// </summary>
    public class CreateAuditEventCommand : IRequest<AuditEventModel>
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public XDASv2Event Event { get; set; }
    }
}
