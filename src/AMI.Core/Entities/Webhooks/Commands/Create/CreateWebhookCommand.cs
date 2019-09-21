using AMI.Core.Entities.Models;
using MediatR;

namespace AMI.Core.Entities.Webhooks.Commands.Create
{
    /// <summary>
    /// A command containing information needed to create a webhook.
    /// </summary>
    public class CreateWebhookCommand : IRequest<WebhookModel>
    {
    }
}
