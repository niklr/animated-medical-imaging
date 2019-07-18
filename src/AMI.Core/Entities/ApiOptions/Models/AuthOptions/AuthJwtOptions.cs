namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The JSON Web Token (JWT) options.
    /// </summary>
    /// <seealso cref="IAuthJwtOptions" />
    public class AuthJwtOptions : IAuthJwtOptions
    {
        /// <inheritdoc/>
        public string SecretKey { get; set; }

        /// <inheritdoc/>
        public string Issuer { get; set; }

        /// <inheritdoc/>
        public string Audience { get; set; }
    }
}
