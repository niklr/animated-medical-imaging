using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;

namespace AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    /// <seealso cref="BaseCommandRequestHandler{ProcessObjectCommand, ProcessObjectTaskModel}" />
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessObjectAsyncCommand, ProcessObjectTaskModel>
    {
        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;
        private readonly ITaskQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="queue">The task queue.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IIdGenService idGenService,
            ITaskQueue queue)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        /// <inheritdoc/>
        protected override async Task<ProcessObjectTaskModel> ProtectedHandleAsync(ProcessObjectAsyncCommand request, CancellationToken cancellationToken)
        {
            context.BeginTransaction();

            var entity = new TaskEntity()
            {
                Id = idGenService.CreateId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = (int)Domain.Enums.TaskStatus.Queued,
                Progress = 0,
                Position = queue.Count,
                CommandType = request.GetType().ToString(),
                CommandSerialized = request.ToString(),
                ObjectId = Guid.Parse(request.Id)
            };

            context.TaskRepository.Add(entity);

            await context.SaveChangesAsync(cancellationToken);

            var result = ProcessObjectTaskModel.Create(entity);
            queue.Add(result);

            context.CommitTransaction();

            return result;
        }
    }
}
