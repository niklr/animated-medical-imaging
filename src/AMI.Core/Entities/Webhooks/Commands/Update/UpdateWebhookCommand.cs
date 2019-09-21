using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Webhooks.Commands.Update
{
    /// <summary>
    /// A command containing information needed to update a webhook.
    /// </summary>
    public class UpdateWebhookCommand : IRequest<WebhookModel>
    {
    }
}
