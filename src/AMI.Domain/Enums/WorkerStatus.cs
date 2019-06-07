namespace AMI.Domain.Enums
{
    /// <summary>
    /// The different states of a worker.
    /// </summary>
    public enum WorkerStatus
    {
        /// <summary>
        /// The worker has been initialized.
        /// </summary>
        Initialized = 0,

        /// <summary>
        /// The worker is currently processing a task.
        /// </summary>
        Processing = 1,

        /// <summary>
        /// The worker is currently idling.
        /// </summary>
        Idling = 2,

        /// <summary>
        /// The worker is currently retrying a failed task.
        /// </summary>
        Retrying = 3,

        /// <summary>
        /// The worker exited with an unexpected exception.
        /// </summary>
        Exception = 4,

        /// <summary>
        /// The worker has been terminated.
        /// </summary>
        Terminated = 5
    }
}
