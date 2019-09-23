using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Extensions.ArrayExtensions;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Webhooks.Commands.Update
{
    /// <summary>
    /// A handler for update command requests.
    /// </summary>
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateWebhookCommand, WebhookModel>
    {
        private readonly IApplicationConstants constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="constants">The application constants.</param>
        public UpdateCommandHandler(ICommandHandlerModule module, IApplicationConstants constants)
            : base(module)
        {
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
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
            var entity = await Context.WebhookRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id));

            if (entity == null)
            {
                throw new NotFoundException(nameof(WebhookEntity), request.Id);
            }

            if (!AuthService.IsAuthorized(entity.UserId))
            {
                throw new ForbiddenException("Not authorized");
            }

            string enabledEvents = request.EnabledEvents.Contains(constants.WildcardCharacter) ?
                constants.WildcardCharacter.Embed(constants.ValueSeparator) :
                request.EnabledEvents.ToArray().ToString(",", constants.ValueSeparator);

            entity.ModifiedDate = DateTime.UtcNow;
            entity.Url = request.Url;
            entity.ApiVersion = request.ApiVersion;
            entity.Secret = request.Secret;
            entity.EnabledEvents = enabledEvents;

            Context.WebhookRepository.Update(entity);

            await Context.SaveChangesAsync(cancellationToken);

            var result = WebhookModel.Create(entity, constants);

            return result;
        }
    }
}
