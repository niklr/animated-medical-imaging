using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AMI.Core.Workers
{
    /// <summary>
    /// A base class for all workers.
    /// </summary>
    public abstract class BaseWorker : IBasicWorker
    {
        private readonly ILogger logger;
        private Stopwatch stopwatch;
        private CancellationTokenSource cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public BaseWorker(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<BaseWorker>();
            Id = Guid.NewGuid();
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
        public DateTime LastActivityDate { get; private set; }

        /// <inheritdoc/>
        public WorkerStatus WorkerStatus { get; private set; }

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

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{GetType().Name} is starting.");

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

                    logger.LogCritical(e, $"Critical exception in {WorkerName}.");
                }
            }

            logger.LogInformation($"{GetType().Name} is stopping.");
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{GetType().Name} stop called.");
            cts?.Cancel();
            logger.LogInformation($"{GetType().Name} stop call ended.");
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
        }
    }
}
