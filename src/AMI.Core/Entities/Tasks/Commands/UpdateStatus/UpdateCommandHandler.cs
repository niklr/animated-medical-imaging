using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Repositories;
using AMI.Domain.Entities;
using AMI.Domain.Exceptions;

namespace AMI.Core.Entities.Tasks.Commands.UpdateStatus
{
    /// <summary>
    /// A handler for command requests to update the status of a task.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{UpdateTaskStatusCommand, TaskModel}" />
    public class UpdateCommandHandler : BaseCommandRequestHandler<UpdateTaskStatusCommand, TaskModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IDefaultJsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public UpdateCommandHandler(IAmiUnitOfWork context, IDefaultJsonSerializer serializer)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        protected override async Task<TaskModel> ProtectedHandleAsync(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.TaskRepository
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

            context.TaskRepository.Update(entity);

            await context.SaveChangesAsync(cancellationToken);

            return TaskModel.Create(entity, serializer);
        }
    }
}
