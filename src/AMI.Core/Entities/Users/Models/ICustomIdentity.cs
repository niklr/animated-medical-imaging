namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing an identity encapsulating information about the user or entity being validated.
    /// </summary>
    public interface ICustomIdentity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        string Domain { get; }
    }
}
