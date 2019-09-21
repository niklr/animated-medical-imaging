using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Webhooks.Commands.Delete
{
    /// <summary>
    /// A command containing information needed to delete a webhook.
    /// </summary>
    public class DeleteWebhookCommand : IRequest<WebhookModel>
    {
    }
}
