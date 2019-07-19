namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The model all tokens have in common.
    /// </summary>
    public abstract class BaseTokenModel
    {
        /// <summary>
        /// Gets or sets the subject claim identifying the principal that is the subject of the token.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Gets or sets the issuer claim identifying the principal that issued the token.
        /// </summary>
        public string Iss { get; set; }

        /// <summary>
        /// Gets or sets the audience claim identifying the recipients that the token is intended for.
        /// </summary>
        public string Aud { get; set; }

        /// <summary>
        /// Gets or sets the "not before" claim identifying the time before which
        /// the token MUST NOT be accepted for processing.
        /// </summary>
        public double Nbf { get; set; }

        /// <summary>
        /// Gets or sets the "issued at" claim identifying the time at which the token was issued.
        /// </summary>
        public double Iat { get; set; }
    }
}
