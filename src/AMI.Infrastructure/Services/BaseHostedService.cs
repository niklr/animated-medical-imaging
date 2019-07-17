using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Workers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A base class for all hosted services.
    /// </summary>
    /// <seealso cref="BackgroundService" />
    public abstract class BaseHostedService : BackgroundService
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected abstract ILogger Logger { get; }

        /// <summary>
        /// Gets the worker.
        /// </summary>
        protected abstract IBaseWorker Worker { get; }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{GetType().Name} stop called.");
            await Worker.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            Logger.LogInformation($"{GetType().Name} stop call ended.");
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{GetType().Name} ExecuteAsync called.");

            try
            {
                Task.Run(() => Worker.StartAsync(cancellationToken));
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }

            Logger.LogInformation($"{GetType().Name} ExecuteAsync ended.");

            return Task.CompletedTask;
        }
    }
}
