using System;
using AMI.Core.Configurations;
using AMI.Core.Workers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A hosted service to cleanup the working directory.
    /// </summary>
    /// <seealso cref="CleanupHostedService" />
    public class CleanupHostedService : BaseHostedService
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
        protected override ILogger Logger => logger;

        /// <inheritdoc/>
        protected override IBaseWorker Worker => worker;
    }
}
