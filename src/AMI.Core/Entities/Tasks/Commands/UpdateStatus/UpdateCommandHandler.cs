using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Entities.Tasks.Queries.GetById;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;

namespace AMI.Core.Entities.Tasks.Commands.UpdateStatus
{
    /// <summary>
    /// A handler for command requests to update the status of a task.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{UpdateTaskStatusCommand, TaskModel}" />
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateTaskStatusCommand, TaskModel>
    {
        private readonly IDefaultJsonSerializer serializer;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="gateway">The gateway service.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="mediator">The mediator.</param>
        public UpdateCommandHandler(
            IAmiUnitOfWork context,
            IGatewayService gateway,
            IDefaultJsonSerializer serializer,
            IMediator mediator)
            : base(context, gateway)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

            entity.ModifiedDate = DateTime.UtcNow;
            entity.Status = (int)request.Status;
            entity.Message = request.Message;

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

            await Gateway.NotifyGroupAsync(
                Gateway.Builder.BuildDefaultGroupName(),
                GatewayOpCode.Dispatch,
                GatewayEvent.UpdateTaskStatus,
                result,
                cancellationToken);

            return result;
        }
    }
}
