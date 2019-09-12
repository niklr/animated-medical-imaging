using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Enums;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A base class for all workers.
    /// </summary>
    public abstract class BaseWorker : IBaseWorker
    {
        private readonly IGatewayService gateway;

        private Stopwatch stopwatch;
        private CancellationTokenSource cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="workerService">The worker service.</param>
        /// <param name="gateway">The gateway service.</param>
        public BaseWorker(ILoggerFactory loggerFactory, IWorkerService workerService, IGatewayService gateway)
        {
            Ensure.ArgumentNotNull(workerService, nameof(workerService));

            Logger = loggerFactory?.CreateLogger<BaseWorker>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            Id = Guid.NewGuid();

            workerService.Add(this);

            this.gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        }

        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public string WorkerName
        {
            get
            {
                return $"{GetType().Name}_{Id}";
            }
        }

        /// <inheritdoc/>
        public abstract WorkerType WorkerType { get; }

        /// <inheritdoc/>
        public WorkerStatus WorkerStatus { get; private set; }

        /// <inheritdoc/>
        public DateTime LastActivityDate { get; private set; }

        /// <inheritdoc/>
        public TimeSpan CurrentProcessingTime
        {
            get
            {
                return stopwatch?.Elapsed ?? TimeSpan.Zero;
            }
        }

        /// <inheritdoc/>
        public TimeSpan LastProcessingTime { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{GetType().Name} start called.");

            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            UpdateActivity(WorkerStatus.Initialized);

            try
            {
                await DoWorkAsync(cancellationToken);

                // worker thread: terminating gracefully.
                UpdateActivity(WorkerStatus.Terminated);
            }
            catch (Exception e)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // worker thread: terminating gracefully.
                    UpdateActivity(WorkerStatus.Terminated);
                }
                else
                {
                    UpdateActivity(WorkerStatus.Exception);

                    Logger.LogCritical(e, $"Critical exception in {WorkerName}.");

                    await StartAsync(cancellationToken);
                }
            }

            Logger.LogInformation($"{GetType().Name} start call ended.");
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{GetType().Name} stop called.");
            cts?.Cancel();
            Logger.LogInformation($"{GetType().Name} stop call ended.");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Does the work asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task DoWorkAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Starts the watch.
        /// </summary>
        protected void StartWatch()
        {
            stopwatch = Stopwatch.StartNew();
            UpdateActivity(WorkerStatus.Processing);
        }

        /// <summary>
        /// Stops the watch.
        /// </summary>
        protected void StopWatch()
        {
            stopwatch?.Stop();
            LastProcessingTime = stopwatch?.Elapsed ?? TimeSpan.Zero;
            stopwatch = null;
            UpdateActivity(WorkerStatus.Idling);
        }

        /// <summary>
        /// Updates the last activity.
        /// </summary>
        /// <param name="status">The status of the worker.</param>
        protected void UpdateActivity(WorkerStatus status)
        {
            LastActivityDate = DateTime.Now;
            WorkerStatus = status;

            Task.Run(
                async () =>
                {
                    await gateway.NotifyGroupsAsync(
                            string.Empty,
                            GatewayOpCode.Dispatch,
                            GatewayEvent.UpdateWorker,
                            BaseWorkerModel.Create(this),
                            cts.Token);
                }, cts.Token);
        }
    }
}
