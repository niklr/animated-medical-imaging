namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing the credentials for login purposes.
    /// </summary>
    public class CredentialsModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
