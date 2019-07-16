using System;
using System.Threading;

namespace AMI.Core.Workers
{
    /// <summary>
    /// An interface representing a recurring worker.
    /// </summary>
    /// <seealso cref="IBaseWorker" />
    public interface IRecurringWorker : IBaseWorker
    {
        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        Timer Timer { get; set; }

        /// <summary>
        /// Gets the next activity date.
        /// </summary>
        DateTime NextActivityDate { get; }
    }
}
