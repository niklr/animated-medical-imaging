using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Entities.Tasks.Queries.GetById;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Enums.Auditing;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.UpdateStatus
{
    /// <summary>
    /// A handler for command requests to update the status of a task.
    /// </summary>
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateTaskStatusCommand, TaskModel>
    {
        private readonly IDefaultJsonSerializer serializer;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="mediator">The mediator.</param>
        public UpdateCommandHandler(
            ICommandHandlerModule module,
            IDefaultJsonSerializer serializer,
            IMediator mediator)
            : base(module)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        protected override SubEventType SubEventType
        {
            get
            {
                return SubEventType.UpdateTask;
            }
        }

        /// <inheritdoc/>
        protected override async Task<TaskModel> ProtectedHandleAsync(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await Context.TaskRepository
                .GetFirstOrDefaultAsync(e => e.Id == Guid.Parse(request.Id));

            if (entity == null)
            {
                throw new NotFoundException(nameof(TaskEntity), request.Id);
            }

            if (!IdentityService.IsAuthorized(entity.UserId))
            {
                throw new ForbiddenException("Not authorized");
            }

            entity.ModifiedDate = DateTime.UtcNow;
            entity.Status = (int)request.Status;
            entity.Message = request.Message;

            switch (request.Status)
            {
                case Domain.Enums.TaskStatus.Created:
                    entity.QueuedDate = null;
                    entity.StartedDate = null;
                    entity.EndedDate = null;
                    break;
                case Domain.Enums.TaskStatus.Queued:
                    entity.QueuedDate = DateTime.UtcNow;
                    entity.StartedDate = null;
                    entity.EndedDate = null;
                    break;
                case Domain.Enums.TaskStatus.Processing:
                    entity.StartedDate = DateTime.UtcNow;
                    entity.EndedDate = null;
                    break;
                case Domain.Enums.TaskStatus.Canceled:
                case Domain.Enums.TaskStatus.Failed:
                case Domain.Enums.TaskStatus.Finished:
                    entity.EndedDate = DateTime.UtcNow;
                    break;
                default:
                    break;
            }

            if (request.Status != Domain.Enums.TaskStatus.Queued)
            {
                entity.Position = 0;
            }

            if (!string.IsNullOrWhiteSpace(request.ResultId))
            {
                entity.ResultId = Guid.Parse(request.ResultId);
            }

            Context.TaskRepository.Update(entity);

            await Context.SaveChangesAsync(cancellationToken);

            var result = await mediator.Send(new GetByIdQuery() { Id = entity.Id.ToString() });

            await Gateway.NotifyGroupsAsync(
                entity.Object?.UserId,
                GatewayOpCode.Dispatch,
                GatewayEvent.UpdateTask,
                result,
                cancellationToken);

            return result;
        }
    }
}
