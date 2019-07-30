using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The model containing information about the identity token.
    /// </summary>
    public class IdTokenModel : BaseTokenModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdTokenModel"/> class.
        /// </summary>
        public IdTokenModel()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email address is confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public IList<string> RoleClaims { get; set; } = new List<string>();
    }
}
