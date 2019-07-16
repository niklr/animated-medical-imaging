using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Configurations;
using AMI.Core.Workers;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A hosted service to cleanup the working directory.
    /// </summary>
    /// <seealso cref="CleanupHostedService" />
    public class CleanupHostedService : BackgroundService
    {
        private readonly ILogger logger;
        private readonly CleanupRecurringWorker worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupHostedService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="configuration">The API configuration.</param>
        public CleanupHostedService(ILoggerFactory loggerFactory, IMediator mediator, IApiConfiguration configuration)
        {
            logger = loggerFactory?.CreateLogger<CleanupHostedService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            worker = new CleanupRecurringWorker(loggerFactory, mediator, configuration);
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
