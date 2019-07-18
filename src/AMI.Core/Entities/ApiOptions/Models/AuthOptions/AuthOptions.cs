using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The options used for authentication and authorization.
    /// </summary>
    /// <seealso cref="IAuthOptions" />
    [Serializable]
    public class AuthOptions : IAuthOptions
    {
        /// <inheritdoc/>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the entities allowed to authenticate.
        /// </summary>
        public IReadOnlyList<AuthEntity> Entities { get; set; }

        IReadOnlyList<IAuthEntity> IAuthOptions.Entities => Entities ?? new List<AuthEntity>();
    }
}
