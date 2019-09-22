using System.Collections.Generic;
using MediatR;

namespace AMI.Core.Entities.Webhooks.Commands
{
    /// <summary>
    /// A command containing information needed to create or update a webhook.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    public abstract class BaseWebhookCommand<T> : IRequest<T>
    {
        /// <summary>
        /// Gets or sets the URL of the webhook endpoint.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the API version used to render events.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the secret used to generate signatures.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the enabled events for this endpoint.
        /// Specify ['*'] to enable all events.
        /// </summary>
        public ISet<string> EnabledEvents { get; set; } = new HashSet<string>();
    }
}
