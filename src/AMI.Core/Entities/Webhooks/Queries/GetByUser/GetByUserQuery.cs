using System.Collections.Generic;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums;
using MediatR;

namespace AMI.Core.Entities.Webhooks.Queries.GetByUser
{
    /// <summary>
    /// An implementation of a query to get webhooks of a user.
    /// </summary>
    public class GetByUserQuery : IRequest<IEnumerable<WebhookModel>>
    {
        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the optional type of the event.
        /// </summary>
        public EventType EventType { get; set; }
    }
}
