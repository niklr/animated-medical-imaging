namespace AMI.Domain.Enums.Auditing
{
    /// <summary>
    /// A type to describe the outcome of an operation.
    /// </summary>
    public enum OutcomeType
    {
        /// <summary>
        /// The operation was successful. 
        /// </summary>
        Success = 0,

        /// <summary>
        /// The operation was erroneous.
        /// </summary>
        Error = 1,

        /// <summary>
        /// The operation was denied.
        /// </summary>
        Denied = 2
    }
}
