namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the gateway events.
    /// </summary>
    public enum GatewayEvent
    {
        /// <summary>
        /// The gateway event is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The event representing a created task.
        /// </summary>
        CreateTask = 1,

        /// <summary>
        /// The event representing an updated task.
        /// </summary>
        UpdateTask = 2,

        /// <summary>
        /// The event representing a deleted task.
        /// </summary>
        DeleteTask = 3,

        /// <summary>
        /// The event representing a created object.
        /// </summary>
        CreateObject = 4,

        /// <summary>
        /// The event representing an updated object.
        /// </summary>
        UpdateObject = 5,

        /// <summary>
        /// The event representing a deleted object.
        /// </summary>
        DeleteObject = 6,

        /// <summary>
        /// The event representing a created audit event.
        /// </summary>
        CreateAuditEvent = 7,

        /// <summary>
        /// The event representing a created worker.
        /// </summary>
        CreateWorker = 8,

        /// <summary>
        /// The event representing an updated worker.
        /// </summary>
        UpdateWorker = 9,

        /// <summary>
        /// The event representing a deleted worker.
        /// </summary>
        DeleteWorkrer = 10
    }
}
