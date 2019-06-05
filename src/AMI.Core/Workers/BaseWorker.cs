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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorker"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public BaseWorker(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<BaseWorker>();
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the identifier of the worker.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name of the worker.
        /// </summary>
        public string WorkerName
        {
            get
            {
                return $"{GetType().Name}_{Id}";
            }
        }

        /// <summary>
        /// Gets the last activity date.
        /// </summary>
        public DateTime LastActivityDate { get; private set; }

        /// <summary>
        /// Gets the current status of the worker.
        /// </summary>
        public WorkerStatus WorkerStatus { get; private set; }

        /// <summary>
        /// Gets the current processing time.
        /// </summary>
        public TimeSpan CurrentProcessingTime
        {
            get
            {
                return stopwatch?.Elapsed ?? TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Gets the last processing time.
        /// </summary>
        public TimeSpan LastProcessingTime { get; private set; }

        /// <summary>
        /// Starts the worker asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{GetType().Name} is starting.");

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
