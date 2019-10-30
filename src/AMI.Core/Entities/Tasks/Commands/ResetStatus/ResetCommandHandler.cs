using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using AMI.Core.Modules;
using AMI.Domain.Enums.Auditing;

namespace AMI.Core.Entities.Tasks.Commands.ResetStatus
{
    /// <summary>
    /// A handler for command requests to reset the status of tasks.
    /// </summary>
    public class ResetCommandHandler : BaseCommandRequestHandler<ResetTaskStatusCommand, bool>
    {
        private readonly IApiConfiguration configuration;
        private readonly IDefaultJsonSerializer serializer;
        private readonly int batchSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetCommandHandler"/> class.
        /// </summary>
        /// <param name="module">The command handler module.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public ResetCommandHandler(
            ICommandHandlerModule module,
            IApiConfiguration configuration,
            IDefaultJsonSerializer serializer)
            : base(module)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            batchSize = configuration.Options.BatchSize > 0 ? configuration.Options.BatchSize : 1000;
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
        protected override async Task<bool> ProtectedHandleAsync(ResetTaskStatusCommand request, CancellationToken cancellationToken)
        {
            await ResetAsync(cancellationToken);

            var entities = Context.TaskRepository.GetQuery(e => e.Status == (int)Domain.Enums.TaskStatus.Queued)
                .OrderBy(e => e.CreatedDate)
                .ThenBy(e => e.Id);

            var iteration = 0;

            while (true)
            {
                var entitiesBatch = entities.Skip(iteration * batchSize).Take(batchSize).ToList();
                if (entitiesBatch.Count <= 0)
                {
                    break;
                }
                else
                {
                    foreach (var entity in entitiesBatch)
                    {
                        var result = TaskModel.Create(entity, serializer);
                    }

                    iteration++;
                }
            }

            return true;
        }

        private async Task ResetAsync(CancellationToken cancellationToken)
        {
            var modifiedDate = DateTime.UtcNow;
            var entities = Context.TaskRepository.GetQuery(e =>
                    e.Status == (int)Domain.Enums.TaskStatus.Created ||
                    e.Status == (int)Domain.Enums.TaskStatus.Queued ||
                    e.Status == (int)Domain.Enums.TaskStatus.Processing)
                .OrderBy(e => e.CreatedDate)
                .ThenBy(e => e.Id);

            var iteration = 0;
            var position = 0;

            while (true)
            {
                var entitiesBatch = entities.Skip(iteration * batchSize).Take(batchSize).ToList();
                if (entitiesBatch.Count <= 0)
                {
                    break;
                }
                else
                {
                    foreach (var entity in entitiesBatch)
                    {
                        entity.ModifiedDate = modifiedDate;
                        entity.QueuedDate = modifiedDate;
                        entity.Status = (int)Domain.Enums.TaskStatus.Queued;
                        entity.Progress = 0;
                        entity.Position = position;

                        position++;
                    }

                    Context.TaskRepository.UpdateRange(entitiesBatch);
                    await Context.SaveChangesAsync(cancellationToken);

                    iteration++;
                }
            }
        }
    }
}
