namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing options related to authentication and authorization.
    /// </summary>
    public interface IAuthOptions
    {
        /// <summary>
        /// Gets a value indicating whether anonymous authentication is allowed.
        /// </summary>
        bool AllowAnonymous { get; }

        /// <summary>
        /// Gets the username for anonymous users (default is Anon).
        /// </summary>
        string AnonymousUsername { get; }

        /// <summary>
        /// Gets the maximum amount of valid refresh tokens a single user is allowed to store (default is 10).
        /// </summary>
        int MaxRefreshTokens { get; }

        /// <summary>
        /// Gets the amount of minutes an access token remains valid (default is 60).
        /// </summary>
        int ExpireAfter { get; }

        /// <summary>
        /// Gets the JSON Web Token (JWT) options.
        /// </summary>
        IAuthJwtOptions JwtOptions { get; }

        /// <summary>
        /// Gets the passwords of system users.
        /// </summary>
        IAuthUserPasswords UserPasswords { get; }
    }
}
