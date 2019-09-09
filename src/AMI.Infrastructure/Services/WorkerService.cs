using System.Collections.Generic;
using System.Collections.ObjectModel;
using AMI.Core.Services;
using AMI.Core.Workers;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The worker service.
    /// </summary>
    public class WorkerService : IWorkerService
    {
        private readonly IList<IBaseWorker> workers = new List<IBaseWorker>();

        /// <inheritdoc/>
        public void Add(IBaseWorker worker)
        {
            workers.Add(worker);
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IBaseWorker> GetWorkers()
        {
            return new ReadOnlyCollection<IBaseWorker>(workers);
        }

        /// <inheritdoc/>
        public void Remove(IBaseWorker worker)
        {
            workers.Remove(worker);
        }
    }
}
