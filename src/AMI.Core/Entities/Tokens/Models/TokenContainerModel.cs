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
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the identifier token.
        /// </summary>
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
