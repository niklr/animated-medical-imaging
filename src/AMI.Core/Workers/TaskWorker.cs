using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Tasks.Commands.ProcessObjectAsync;
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
            this.queue = queue;
            this.mediator = mediator;
        }

        /// <inheritdoc/>
        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            foreach (var item in queue.GetConsumingEnumerable(cancellationToken))
            {
                StartWatch();

                switch (item.CommandType)
                {
                    case CommandType.ProcessObjectAsyncCommand:
                        await ProcessObjectAsync(item, cancellationToken);
                        break;
                    default:
                        // TODO: implement default behavior
                        break;
                }

                StopWatch();
            }
        }

        private async Task ProcessObjectAsync(TaskModel item, CancellationToken cancellationToken)
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

                var result = await mediator.Send(command, cancellationToken);

                // TODO: update task
            }
            catch (Exception)
            {
                // TODO: log exception and update task
            }
        }
    }
}
