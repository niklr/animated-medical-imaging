using System;
using AMI.Core.Configurations;
using AMI.Core.Queues;
using AMI.Core.Services;
using AMI.Core.Workers;
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
        private readonly QueueWorker worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessTaskHostedService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="workerService">The worker service.</param>
        public ProcessTaskHostedService(
            ILoggerFactory loggerFactory,
            IAppConfiguration configuration,
            ITaskQueue queue,
            IServiceProvider serviceProvider,
            IWorkerService workerService)
        {
            logger = loggerFactory?.CreateLogger<ProcessTaskHostedService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            worker = new QueueWorker(loggerFactory, workerService, configuration, queue, serviceProvider);
        }

        /// <inheritdoc/>
        protected override ILogger Logger => logger;

        /// <inheritdoc/>
        protected override IBaseWorker Worker => worker;
    }
}
