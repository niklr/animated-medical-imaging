using System;
using System.Collections.Generic;
using System.Threading;
using AMI.Core.Entities.Results.Commands.ProcessObjects;

namespace AMI.Core.Queues
{
    /// <summary>
    /// A queue to process objects.
    /// </summary>
    public interface IProcessObjectQueue : IDisposable
    {
        /// <summary>
        /// Gets the number of items contained in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a consuming enumerable for the items in the queue.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>An enumerable that removes and returns items from the queue.</returns>
        IEnumerable<ProcessObjectCommand> GetConsumingEnumerable(CancellationToken token);
    }
}
