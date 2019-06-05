using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Services
{
    /// <summary>
    /// A hosted service to process objects.
    /// </summary>
    /// <seealso cref="BackgroundService" />
    public class ProcessObjectHostedService : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IProcessObjectWorker worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectHostedService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="worker">The worker to process objects.</param>
        public ProcessObjectHostedService(ILoggerFactory loggerFactory, IProcessObjectWorker worker)
        {
            logger = loggerFactory.CreateLogger<ProcessObjectHostedService>();
            this.worker = worker;
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
