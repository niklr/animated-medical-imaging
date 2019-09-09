namespace AMI.Core.Workers
{
    /// <summary>
    /// An interface representing a queue worker.
    /// </summary>
    /// <seealso cref="IBaseWorker" />
    public interface IQueueWorker : IBaseWorker
    {
        /// <summary>
        /// Gets the queue count.
        /// </summary>
        int Count { get; }
    }
}
