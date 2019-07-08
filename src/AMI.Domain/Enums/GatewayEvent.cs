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
        /// The event representing an update of a task status.
        /// </summary>
        UpdateTaskStatus = 1
    }
}
