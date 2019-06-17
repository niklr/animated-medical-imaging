using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;
using AMI.Core.Entities.Tasks.Commands.UpdateStatus;
using AMI.Core.Queues;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A worker to process tasks.
    /// </summary>
    /// <seealso cref="BaseWorker" />
    /// <seealso cref="ITaskWorker" />
    public class TaskWorker : BaseWorker, ITaskWorker
    {
        private readonly ILogger logger;
        private readonly ITaskQueue queue;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="mediator">The mediator.</param>
        public TaskWorker(ILoggerFactory loggerFactory, ITaskQueue queue, IMediator mediator)
            : base(loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<TaskWorker>();

            this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <inheritdoc/>
        protected override async Task DoWorkAsync(CancellationToken ct)
        {
            foreach (var item in queue.GetConsumingEnumerable(ct))
            {
                ct.ThrowIfCancellationRequested();

                StartWatch();

                await UpdateStatus(item, Domain.Enums.TaskStatus.Processing, string.Empty, ct);
                await UpdatePositionsAsync();

                if (item.Command != null)
                {
                    switch (item.Command.CommandType)
                    {
                        case CommandType.ProcessObjectAsyncCommand:
                            await ProcessObjectAsync(item, ct);
                            break;
                        default:
                            await UpdateStatus(item, Domain.Enums.TaskStatus.Finished, string.Empty, ct);
                            break;
                    }
                }

                StopWatch();
            }
        }

        private async Task ProcessObjectAsync(TaskModel item, CancellationToken ct)
        {
            try
            {
                if (item.Command == null)
                {
                    throw new UnexpectedNullException($"The command of task {item.Id} is null.");
                }

                if (item.Command.GetType() != typeof(ProcessObjectAsyncCommand))
                {
                    throw new AmiException($"Task {item.Id} has an unexpected command type.");
                }

                var castedCommand = (ProcessObjectAsyncCommand)item.Command;

                var command = new ProcessObjectCommand()
                {
                    Id = castedCommand.Id,
                    DesiredSize = castedCommand.DesiredSize,
                    AmountPerAxis = castedCommand.AmountPerAxis,
                    AxisTypes = castedCommand.AxisTypes,
                    ImageFormat = castedCommand.ImageFormat,
                    BezierEasingTypePerAxis = castedCommand.BezierEasingTypePerAxis,
                    BezierEasingTypeCombined = castedCommand.BezierEasingTypeCombined,
                    Grayscale = castedCommand.Grayscale
                };

                var result = await mediator.Send(command, ct);

                await UpdateStatus(item, Domain.Enums.TaskStatus.Finished, string.Empty, ct);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation($"Processing of task {item.Id} canceled.");
                await UpdateStatus(item, Domain.Enums.TaskStatus.Canceled, string.Empty, ct);
            }
            catch (Exception e)
            {
                logger.LogInformation($"Processing of task {item.Id} failed. {e.Message}");
                await UpdateStatus(item, Domain.Enums.TaskStatus.Failed, e.Message, ct);
            }
        }

        private async Task UpdateStatus(TaskModel task, Domain.Enums.TaskStatus status, string message, CancellationToken ct)
        {
            var command = new UpdateTaskStatusCommand()
            {
                Id = task.Id,
                Status = status,
                Message = message
            };

            await mediator.Send(command, ct);
        }

        private async Task UpdatePositionsAsync()
        {
            // TODO: Decrease all positions by 1
        }
    }
}
