using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Models;

namespace AMI.Core.Queues
{
    /// <summary>
    /// A queue for tasks.
    /// </summary>
    /// <seealso cref="ITaskQueue" />
    public class TaskQueue : ITaskQueue
    {
        private readonly BlockingCollection<TaskModel> queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskQueue"/> class.
        /// </summary>
        public TaskQueue()
        {
            queue = new BlockingCollection<TaskModel>();
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        /// <inheritdoc/>
        public void Add(TaskModel task)
        {
            queue.Add(task);
        }

        /// <inheritdoc/>
        public IEnumerable<TaskModel> GetConsumingEnumerable(CancellationToken token)
        {
            return queue.GetConsumingEnumerable(token);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            queue.Dispose();
        }
    }
}
