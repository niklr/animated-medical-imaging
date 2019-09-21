namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe webhook/gateway events.
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// The event type is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The event representing a created task.
        /// </summary>
        TaskCreated = 10,

        /// <summary>
        /// The event representing an updated task.
        /// </summary>
        TaskUpdated = 11,

        /// <summary>
        /// The event representing a deleted task.
        /// </summary>
        TaskDeleted = 12,

        /// <summary>
        /// The event representing a created object.
        /// </summary>
        ObjectCreated = 20,

        /// <summary>
        /// The event representing an updated object.
        /// </summary>
        ObjectUpdated = 21,

        /// <summary>
        /// The event representing a deleted object.
        /// </summary>
        ObjectDeleted = 22,

        /// <summary>
        /// The event representing a created audit event.
        /// </summary>
        AuditEventCreated = 30,

        /// <summary>
        /// The event representing a created worker.
        /// </summary>
        WorkerCreated = 40,

        /// <summary>
        /// The event representing an updated worker.
        /// </summary>
        WorkerUpdated = 41,

        /// <summary>
        /// The event representing a deleted worker.
        /// </summary>
        WorkerDeleted = 42
    }
}
