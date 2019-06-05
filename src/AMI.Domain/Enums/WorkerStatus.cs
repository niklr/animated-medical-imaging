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
        Initialized,
        /// <summary>
        /// The worker is currently processing a task.
        /// </summary>
        Processing,
        /// <summary>
        /// The worker is currently idling.
        /// </summary>
        Idling,
        /// <summary>
        /// The worker is currently retrying a failed task.
        /// </summary>
        Retrying,
        /// <summary>
        /// The worker exited with an unexpected exception.
        /// </summary>
        Exception,
        /// <summary>
        /// The worker has been terminated.
        /// </summary>
        Terminated
    }
}
