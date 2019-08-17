namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The model containing information about the access token.
    /// </summary>
    public class AccessTokenModel : BaseTokenModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenModel"/> class.
        /// </summary>
        public AccessTokenModel()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the "expiration time" claim identifying the expiration time on
        /// or after which the token MUST NOT be accepted for processing.
        /// </summary>
        public double Exp { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
    }
}
