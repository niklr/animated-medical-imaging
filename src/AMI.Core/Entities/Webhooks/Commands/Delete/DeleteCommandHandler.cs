using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Entities.Webhooks.Commands.Delete
{
    /// <summary>
    /// A handler for delete command requests.
    /// </summary>
    public class DeleteCommandHandler : BaseCommandRequestHandler<DeleteWebhookCommand, WebhookModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        public DeleteCommandHandler(ICommandHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.DeleteWebhook;
            }
        }

        /// <inheritdoc/>
        protected override async Task<WebhookModel> ProtectedHandleAsync(DeleteWebhookCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
