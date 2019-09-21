using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Entities.Webhooks.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateWebhookCommand, WebhookModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        public CreateCommandHandler(ICommandHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.CreateWebhook;
            }
        }

        /// <inheritdoc/>
        protected override async Task<WebhookModel> ProtectedHandleAsync(CreateWebhookCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
