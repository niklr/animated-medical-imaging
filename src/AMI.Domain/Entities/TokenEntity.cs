using System;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a token related to authentication and authorization.
    /// </summary>
    public class TokenEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last used date.
        /// </summary>
        public DateTime LastUsedDate { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public string TokenValue { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserEntity User { get; set; }
    }
}
