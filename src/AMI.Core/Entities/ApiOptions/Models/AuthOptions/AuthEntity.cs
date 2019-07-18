using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing an entity related to authentication and authorization.
    /// </summary>
    [Serializable]
    public class AuthEntity : IAuthEntity
    {
        /// <inheritdoc/>
        public string Username { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public IReadOnlyList<string> Roles { get; set; }

        IReadOnlyList<string> IAuthEntity.Roles => Roles ?? new List<string>();
    }
}
