using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Queues;
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

                switch (item)
                {
                    case ProcessObjectTaskModel processObject:
                        await ProcessObjectTaskAsync(processObject, cancellationToken);
                        break;
                    default:
                        throw new NotSupportedException("Process command type is not supported.");
                }

                StopWatch();
            }
        }

        private async Task ProcessObjectTaskAsync(ProcessObjectTaskModel item, CancellationToken cancellationToken)
        {
            var command = new ProcessObjectCommand()
            {
                Id = item.Id,
                DesiredSize = item.Command.DesiredSize,
                AmountPerAxis = item.Command.AmountPerAxis,
                AxisTypes = item.Command.AxisTypes,
                ImageFormat = item.Command.ImageFormat,
                BezierEasingTypePerAxis = item.Command.BezierEasingTypePerAxis,
                BezierEasingTypeCombined = item.Command.BezierEasingTypeCombined,
                Grayscale = item.Command.Grayscale
            };

            await mediator.Send(command, cancellationToken);

            // TODO: store result or error
        }
    }
}
