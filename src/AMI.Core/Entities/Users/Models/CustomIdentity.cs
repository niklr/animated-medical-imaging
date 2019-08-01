namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An identity encapsulating information about the user or entity being validated.
    /// </summary>
    public class CustomIdentity : ICustomIdentity
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Username { get; set; }

        /// <inheritdoc/>
        public string Domain { get; set; }

        /// <inheritdoc/>
        public string AuthenticationType => string.Empty;

        /// <inheritdoc/>
        public bool IsAuthenticated => true;
    }
}
