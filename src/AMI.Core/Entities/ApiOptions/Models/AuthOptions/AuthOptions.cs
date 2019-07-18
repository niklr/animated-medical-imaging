using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        /// <inheritdoc/>
        [IgnoreDataMember]
        public IAuthJwtOptions JwtOptions { get; set; } = new AuthJwtOptions();

        /// <summary>
        /// Gets or sets the entities allowed to authenticate.
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyList<AuthEntity> Entities { get; set; }

        [IgnoreDataMember]
        IReadOnlyList<IAuthEntity> IAuthOptions.Entities => Entities ?? new List<AuthEntity>();
    }
}
