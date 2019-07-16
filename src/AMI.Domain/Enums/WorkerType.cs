namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the worker.
    /// </summary>
    public enum WorkerType
    {
        /// <summary>
        /// The type of the worker is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The default worker type.
        /// </summary>
        Default = 1,

        /// <summary>
        /// The recurring worker type.
        /// </summary>
        Recurring = 2
    }
}
