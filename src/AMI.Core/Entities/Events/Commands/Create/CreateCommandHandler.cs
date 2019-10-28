using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Entities.Events.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateEventCommand, EventModel>
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
        protected override async Task<EventModel> ProtectedHandleAsync(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = new EventEntity()
            {
                Id = idGenerator.GenerateId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                EventType = (int)request.EventType,
                UserId = request.UserId,
                EventSerialized = serializer.Serialize(request.Event)
            };

            Context.EventRepository.Add(eventEntity);

            await Context.SaveChangesAsync(cancellationToken);

            var result = EventModel.Create(eventEntity, serializer);

            return result;
        }
    }
}
