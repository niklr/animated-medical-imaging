namespace AMI.Domain.Enums
{
    /// <summary>
    /// The different states of a task.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task has been initialized but has not yet been scheduled.
        /// </summary>
        Created = 0,

        /// <summary>
        /// The task has been scheduled for execution but has not yet begun executing.
        /// </summary>
        Queued = 1,

        /// <summary>
        /// The task is processing but has not yet completed.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// The task completed due to cancellation.
        /// </summary>
        Canceled = 3,

        /// <summary>
        /// The task completed due to an unhandled exception.
        /// </summary>
        Failed = 4,

        /// <summary>
        /// The task completed execution successfully.
        /// </summary>
        Finished = 5
    }
}
