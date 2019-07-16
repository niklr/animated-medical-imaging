using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class ProcessTaskHostedService : BackgroundService
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
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{GetType().Name} stop called.");
            await worker.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            logger.LogInformation($"{GetType().Name} stop call ended.");
        }

        /// <inheritdoc/>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{GetType().Name} is starting.");

            try
            {
                await worker.StartAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            logger.LogInformation($"{GetType().Name} is stopping.");
        }
    }
}
