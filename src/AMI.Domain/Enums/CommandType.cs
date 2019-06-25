namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the command.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// The type of the command is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The command type to process paths.
        /// </summary>
        ProcessPathCommand = 1,

        /// <summary>
        /// The command type to process objects.
        /// </summary>
        ProcessObjectCommand = 2
    }
}
