using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a webhook endpoint.
    /// </summary>
    public class WebhookEntity
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
        /// Gets or sets the enabled events separated by a comma.
        /// </summary>
        public string EnabledEvents { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }
    }
}
