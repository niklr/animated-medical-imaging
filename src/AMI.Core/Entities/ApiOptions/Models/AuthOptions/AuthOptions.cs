using System;
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
        public string AnonymousUsername { get; set; } = "Anon";

        /// <inheritdoc/>
        public int MaxRefreshTokens { get; set; } = 10;

        /// <inheritdoc/>
        public int ExpireAfter { get; set; } = 60;

        /// <inheritdoc/>
        [IgnoreDataMember]
        public IAuthJwtOptions JwtOptions { get; set; } = new AuthJwtOptions();

        /// <inheritdoc/>
        [IgnoreDataMember]
        public IAuthUserPasswords UserPasswords { get; set; } = new AuthUserPasswords();
    }
}
