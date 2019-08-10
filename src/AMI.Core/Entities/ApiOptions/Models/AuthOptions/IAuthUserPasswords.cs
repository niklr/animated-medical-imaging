namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing passwords used to configure system users.
    /// </summary>
    public interface IAuthUserPasswords
    {
        /// <summary>
        /// Gets the password for the service system user.
        /// </summary>
        string Svc { get; }

        /// <summary>
        /// Gets the password for the administrator system user.
        /// </summary>
        string Admin { get; }
    }
}
