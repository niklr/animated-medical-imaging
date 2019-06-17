using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Queues;
using AMI.Core.Repositories;
using AMI.Core.Services;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync
{
    /// <summary>
    /// A handler for process command requests.
    /// </summary>
    public class ProcessCommandHandler : BaseCommandRequestHandler<ProcessObjectAsyncCommand, TaskModel>
    {
        private static Mutex processMutex;

        private readonly IAmiUnitOfWork context;
        private readonly IIdGenService idGenService;
        private readonly IDefaultJsonSerializer serializer;
        private readonly ITaskQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="idGenService">The service to generate unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="queue">The task queue.</param>
        public ProcessCommandHandler(
            IAmiUnitOfWork context,
            IIdGenService idGenService,
            IDefaultJsonSerializer serializer,
            ITaskQueue queue)
            : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.idGenService = idGenService ?? throw new ArgumentNullException(nameof(idGenService));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        /// <inheritdoc/>
        protected override async Task<TaskModel> ProtectedHandleAsync(ProcessObjectAsyncCommand request, CancellationToken cancellationToken)
        {
            processMutex = new Mutex(false, this.GetMethodName());

            return await processMutex.Execute(new TimeSpan(0, 0, 1), async () =>
            {
                context.BeginTransaction();

                Guid objectId = Guid.Parse(request.Id);

                var activeCount = await context.TaskRepository.CountAsync(e =>
                    e.ObjectId == objectId &&
                    (e.Status == (int)Domain.Enums.TaskStatus.Created ||
                    e.Status == (int)Domain.Enums.TaskStatus.Queued ||
                    e.Status == (int)Domain.Enums.TaskStatus.Processing));

                if (activeCount > 0)
                {
                    throw new AmiException("The specified object is already actively being processed.");
                }

                var entity = new TaskEntity()
                {
                    Id = idGenService.CreateId(),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Domain.Enums.TaskStatus.Queued,
                    Progress = 0,
                    Position = queue.Count,
                    CommandType = (int)CommandType.ProcessObjectAsyncCommand,
                    CommandSerialized = serializer.Serialize(request),
                    ObjectId = objectId
                };

                context.TaskRepository.Add(entity);

                await context.SaveChangesAsync(cancellationToken);

                var result = TaskModel.Create(entity, serializer);
                queue.Add(result);

                context.CommitTransaction();

                return result;
            });
        }
    }
}
