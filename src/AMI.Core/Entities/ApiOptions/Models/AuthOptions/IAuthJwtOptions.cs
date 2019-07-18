namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing JSON Web Token (JWT) options.
    /// </summary>
    public interface IAuthJwtOptions
    {
        /// <summary>
        /// Gets the secret key used to sign created tokens and to validate received tokens.
        /// </summary>
        string SecretKey { get; }

        /// <summary>
        /// Gets the principal that issued the JWT.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Gets the recipient that the JWT is intended for.
        /// </summary>
        string Audience { get; }

        /// <summary>
        /// Gets the claim representing the name.
        /// </summary>
        string NameClaimType { get; }

        /// <summary>
        /// Gets the claim representing the role.
        /// </summary>
        string RoleClaimType { get; }

        /// <summary>
        /// Gets the claim representing the issuer.
        /// </summary>
        string IssuerClaimType { get; }

        /// <summary>
        /// Gets the claim representing the username.
        /// </summary>
        string UsernameClaimType { get; }
    }
}
