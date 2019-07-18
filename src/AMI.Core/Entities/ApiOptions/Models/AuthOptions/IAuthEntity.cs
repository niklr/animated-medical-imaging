using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing an entity related to authentication.
    /// </summary>
    public interface IAuthEntity
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        IReadOnlyList<string> Roles { get; }
    }
}
