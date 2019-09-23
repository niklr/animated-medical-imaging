using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Modules;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Webhooks.Commands.Delete
{
    /// <summary>
    /// A handler for delete command requests.
    /// </summary>
    public class DeleteCommandHandler : BaseCommandRequestHandler<DeleteWebhookCommand, bool>
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
        protected override async Task<bool> ProtectedHandleAsync(DeleteWebhookCommand request, CancellationToken cancellationToken)
        {
            var entity = await Context.WebhookRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id), cancellationToken);

            if (entity == null)
            {
                return true;
            }

            if (!AuthService.IsAuthorized(entity.UserId))
            {
                throw new ForbiddenException("Not authorized");
            }

            Context.WebhookRepository.Remove(entity);

            await Context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
