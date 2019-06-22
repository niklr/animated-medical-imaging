using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A hosted service to process objects.
    /// </summary>
    /// <seealso cref="BackgroundService" />
    public class ProcessObjectHostedService : BackgroundService
    {
        private readonly ILogger logger;
        private readonly ITaskWorker worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectHostedService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="worker">The worker to process objects.</param>
        public ProcessObjectHostedService(ILoggerFactory loggerFactory, ITaskWorker worker)
        {
            logger = loggerFactory.CreateLogger<ProcessObjectHostedService>();
            this.worker = worker;
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
