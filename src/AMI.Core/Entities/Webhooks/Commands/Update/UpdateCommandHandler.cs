using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Entities.Webhooks.Commands.Update
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateWebhookCommand, WebhookModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        public UpdateCommandHandler(ICommandHandlerModule module)
            : base(module)
        {
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.UpdateWebhook;
            }
        }

        /// <inheritdoc/>
        protected override async Task<WebhookModel> ProtectedHandleAsync(UpdateWebhookCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
