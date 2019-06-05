using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Queues;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A worker to process objects.
    /// </summary>
    /// <seealso cref="BaseWorker" />
    /// <seealso cref="IProcessObjectWorker" />
    public class ProcessObjectWorker : BaseWorker, IProcessObjectWorker
    {
        private readonly IProcessObjectQueue queue;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="mediator">The mediator.</param>
        public ProcessObjectWorker(ILoggerFactory loggerFactory, IProcessObjectQueue queue, IMediator mediator)
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

                await mediator.Send(item, cancellationToken);

                StopWatch();
            }
        }
    }
}
