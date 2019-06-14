using System;
using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Models;

namespace AMI.Core.Queues
{
    /// <summary>
    /// A queue for tasks.
    /// </summary>
    public interface ITaskQueue : IDisposable
    {
        /// <summary>
        /// Gets the number of items contained in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds the specified task to the queue.
        /// </summary>
        /// <param name="task">The task.</param>
        void Add(TaskModel task);

        /// <summary>
        /// Gets a consuming enumerable for the items in the queue.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>An enumerable that removes and returns items from the queue.</returns>
        IEnumerable<TaskModel> GetConsumingEnumerable(CancellationToken token);
    }
}
