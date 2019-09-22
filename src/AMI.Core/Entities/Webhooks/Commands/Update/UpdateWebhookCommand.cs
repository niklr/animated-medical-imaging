using AMI.Core.Entities.Models;

namespace AMI.Core.Entities.Webhooks.Commands.Update
{
    /// <summary>
    /// A command containing information needed to update a webhook.
    /// </summary>
    public class UpdateWebhookCommand : BaseWebhookCommand<WebhookModel>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
