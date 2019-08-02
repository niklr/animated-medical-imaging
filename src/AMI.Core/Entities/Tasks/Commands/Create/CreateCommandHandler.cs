﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Generators;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Core.Queues;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using RNS.Framework.Extensions.MutexExtensions;
using RNS.Framework.Extensions.Reflection;

namespace AMI.Core.Entities.Tasks.Commands.Create
{
    /// <summary>
    /// A handler for create command requests.
    /// </summary>
    public class CreateCommandHandler : BaseCommandRequestHandler<CreateTaskCommand, TaskModel>
    {
        private static Mutex processMutex;

        private readonly IIdGenerator idGenerator;
        private readonly IDefaultJsonSerializer serializer;
        private readonly ITaskQueue queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="idGenerator">The generator for unique identifiers.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <param name="queue">The task queue.</param>
        public CreateCommandHandler(
            ICommandHandlerModule module,
            IIdGenerator idGenerator,
            IDefaultJsonSerializer serializer,
            ITaskQueue queue)
            : base(module)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        /// <inheritdoc/>
        protected override async Task<TaskModel> ProtectedHandleAsync(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var principal = PrincipalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new ForbiddenException("Not authenticated");
            }

            if (request.Command.CommandType != CommandType.ProcessObjectCommand || request.Command.GetType() != typeof(ProcessObjectCommand))
            {
                throw new NotSupportedException("The provided command type is not supported.");
            }

            var command = request.Command as ProcessObjectCommand;

            Guid objectId = Guid.Parse(command.Id);
            var objectOwnerId = Context.ObjectRepository.GetQuery(e => e.Id == objectId).Select(e => e.UserId).FirstOrDefault();

            if (!IdentityService.IsAuthorized(objectOwnerId))
            {
                throw new ForbiddenException("Not authorized");
            }

            processMutex = new Mutex(false, this.GetMethodName());

            return await processMutex.Execute(new TimeSpan(0, 0, 2), async () =>
            {
                Context.BeginTransaction();

                var activeCount = await Context.TaskRepository.CountAsync(e =>
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
                    Id = idGenerator.GenerateId(),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = (int)Domain.Enums.TaskStatus.Queued,
                    Progress = 0,
                    Position = queue.Count,
                    CommandType = (int)CommandType.ProcessObjectCommand,
                    CommandSerialized = serializer.Serialize(command),
                    UserId = principal.Identity.Name,
                    ObjectId = objectId
                };

                Context.TaskRepository.Add(entity);

                await Context.SaveChangesAsync(cancellationToken);

                var result = TaskModel.Create(entity, serializer);
                queue.Add(result);

                Context.CommitTransaction();

                return result;
            });
        }
    }
}
