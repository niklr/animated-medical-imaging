using System;
using AMI.Core.Queues;
using AMI.Core.Workers;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A hosted service to process tasks.
    /// </summary>
    /// <seealso cref="BackgroundService" />
    public class ProcessTaskHostedService : BaseHostedService
    {
        private readonly ILogger logger;
        private readonly TaskWorker worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessTaskHostedService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="queue">The queue.</param>
        public ProcessTaskHostedService(ILoggerFactory loggerFactory, IMediator mediator, ITaskQueue queue)
        {
            logger = loggerFactory?.CreateLogger<ProcessTaskHostedService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            worker = new TaskWorker(loggerFactory, mediator, queue);
        }

        /// <inheritdoc/>
        protected override ILogger Logger => logger;

        /// <inheritdoc/>
        protected override IBaseWorker Worker => worker;
    }
}
