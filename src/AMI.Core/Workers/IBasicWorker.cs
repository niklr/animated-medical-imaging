using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Enums;

namespace AMI.Core.Workers
{
    /// <summary>
    /// An interface representing a basic worker.
    /// </summary>
    public interface IBasicWorker
    {
        /// <summary>
        /// Gets the identifier of the worker.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of the worker.
        /// </summary>
        string WorkerName { get; }

        /// <summary>
        /// Gets the last activity date.
        /// </summary>
        DateTime LastActivityDate { get; }

        /// <summary>
        /// Gets the current status of the worker.
        /// </summary>
        WorkerStatus WorkerStatus { get; }

        /// <summary>
        /// Gets the current processing time.
        /// </summary>
        TimeSpan CurrentProcessingTime { get; }

        /// <summary>
        /// Gets the last processing time.
        /// </summary>
        TimeSpan LastProcessingTime { get; }

        /// <summary>
        /// Starts the worker asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the worker asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
