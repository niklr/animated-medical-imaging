using System;
using System.Collections.Generic;

namespace AMI.Domain.Entities
{
    /// <summary>
    /// An entity representing a user.
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEntity"/> class.
        /// </summary>
        public UserEntity()
        {
            Tokens = new HashSet<TokenEntity>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the normalized username.
        /// </summary>
        public string NormalizedUsername { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address.
        /// </summary>
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email address is confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the roles separated by a comma.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        public ICollection<TokenEntity> Tokens { get; set; }
    }
}
