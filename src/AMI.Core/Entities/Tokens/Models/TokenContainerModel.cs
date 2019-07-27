namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// A model containing the access, identifier and refresh tokens.
    /// </summary>
    public class TokenContainerModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public AccessTokenModel AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the endcoded access token.
        /// </summary>
        public string AccessTokenEncoded { get; set; }

        /// <summary>
        /// Gets or sets the identity token.
        /// </summary>
        public IdTokenModel IdToken { get; set; }

        /// <summary>
        /// Gets or sets the endcoded identity token.
        /// </summary>
        public string IdTokenEncoded { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public RefreshTokenModel RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the encoded refresh token.
        /// </summary>
        public string RefreshTokenEncoded { get; set; }
    }
}
