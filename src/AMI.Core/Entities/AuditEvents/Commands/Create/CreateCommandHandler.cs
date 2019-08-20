using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Enums.Auditing;
using XDASv2Net.Model;

namespace AMI.Core.Entities.AuditEvents.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateAuditEventCommand, AuditEventModel>
    {
        private readonly IIdGenerator idGenerator;
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public CreateCommandHandler(
            ICommandHandlerModule module,
            IIdGenerator idGenerator,
            IDefaultJsonSerializer serializer)
            : base(module)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.None;
            }
        }

        /// <inheritdoc/>
        protected override async Task<AuditEventModel> ProtectedHandleAsync(CreateAuditEventCommand request, CancellationToken cancellationToken)
        {
            EventType parsedEventType = Enum.TryParse(request.Event.Action.Event.Name, out EventType eventType)
                 ? eventType : EventType.INVOKE_SERVICE;
            SubEventType parsedSubEventType = Enum.TryParse(request.Event.Action.SubEvent.Name, out SubEventType subEventType)
                 ? subEventType : SubEventType.None;

            var eventEntity = new AuditEventEntity()
            {
                Id = idGenerator.GenerateId(),
                Timestamp = DateTime.UtcNow,
                EventType = (int)parsedEventType,
                SubEventType = (int)parsedSubEventType,
                EventSerialized = serializer.Serialize(request.Event)
            };

            Context.AuditEventRepository.Add(eventEntity);

            await Context.SaveChangesAsync(cancellationToken);

            var result = AuditEventModel.Create(eventEntity, serializer);

            await Gateway.NotifyGroupsAsync(
                string.Empty,
                GatewayOpCode.Dispatch,
                GatewayEvent.CreateAuditEvent,
                result,
                cancellationToken);

            return result;
        }
    }
}
