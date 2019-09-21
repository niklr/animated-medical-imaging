namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing information about the webhook endpoint.
    /// </summary>
    public class WebhookModel
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
        public string[] EnabledEvents { get; set; }
    }
}
