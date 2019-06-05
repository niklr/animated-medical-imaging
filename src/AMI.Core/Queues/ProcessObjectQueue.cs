using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Objects.Commands.Process;

namespace AMI.Core.Queues
{
    /// <summary>
    /// A queue to process objects.
    /// </summary>
    /// <seealso cref="IProcessObjectQueue" />
    public class ProcessObjectQueue : IProcessObjectQueue
    {
        private readonly BlockingCollection<ProcessObjectCommand> queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectQueue"/> class.
        /// </summary>
        public ProcessObjectQueue()
        {
            queue = new BlockingCollection<ProcessObjectCommand>();
        }

        /// <summary>
        /// Gets the number of items contained in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        /// <summary>
        /// Gets a consuming enumerable for the items in the queue.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>An enumerable that removes and returns items from the queue.</returns>
        public IEnumerable<ProcessObjectCommand> GetConsumingEnumerable(CancellationToken token)
        {
            return queue.GetConsumingEnumerable(token);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            queue.Dispose();
        }
    }
}
