using System.Collections.Generic;
using AMI.Core.Workers;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a worker service.
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Adds the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        void Add(IBaseWorker worker);

        /// <summary>
        /// Gets the workers.
        /// </summary>
        /// <returns>A read-only collection of workers.</returns>
        IReadOnlyCollection<IBaseWorker> GetWorkers();

        /// <summary>
        /// Removes the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        void Remove(IBaseWorker worker);
    }
}
