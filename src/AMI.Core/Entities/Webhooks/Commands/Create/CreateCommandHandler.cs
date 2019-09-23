using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Extensions.ArrayExtensions;
using AMI.Core.Extensions.StringExtensions;
using AMI.Core.IO.Generators;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Core.Entities.Webhooks.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateWebhookCommand, WebhookModel>
    {
        private static Mutex processMutex;

        private readonly IIdGenerator idGenerator;
        private readonly IApplicationConstants constants;
        private readonly IApiConfiguration apiConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="constants">The application constants.</param>
        /// <param name="apiConfiguration">The API configuration.</param>
        public CreateCommandHandler(
            ICommandHandlerModule module,
            IIdGenerator idGenerator,
            IApplicationConstants constants,
            IApiConfiguration apiConfiguration)
            : base(module)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
            this.apiConfiguration = apiConfiguration ?? throw new ArgumentNullException(nameof(apiConfiguration));
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
            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            int webhookLimit;
            if (principal.IsInRole(RoleType.Administrator))
            {
                webhookLimit = 0;
            }
            else
            {
                webhookLimit = apiConfiguration.Options.WebhookLimit;
            }

            processMutex = new Mutex(false, this.GetMethodName());

            return await processMutex.Execute(new TimeSpan(0, 0, 2), async () =>
            {
                if (webhookLimit > 0)
                {
                    var count = await Context.WebhookRepository.CountAsync(
                        e => e.UserId == principal.Identity.Name, cancellationToken);
                    if (count >= webhookLimit)
                    {
                        throw new AmiException($"The webhook limit of {webhookLimit} has been reached.");
                    }
                }

                Guid guid = idGenerator.GenerateId();

                string enabledEvents = request.EnabledEvents.Contains(constants.WildcardCharacter) ?
                    constants.WildcardCharacter.Embed(constants.ValueSeparator) :
                    request.EnabledEvents.ToArray().ToString(",", constants.ValueSeparator);

                var entity = new WebhookEntity()
                {
                    Id = guid,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Url = request.Url,
                    ApiVersion = request.ApiVersion,
                    Secret = request.Secret,
                    EnabledEvents = enabledEvents,
                    UserId = principal.Identity.Name
                };

                Context.WebhookRepository.Add(entity);

                await Context.SaveChangesAsync(cancellationToken);

                var result = WebhookModel.Create(entity, constants);

                return result;
            });
        }
    }
}
