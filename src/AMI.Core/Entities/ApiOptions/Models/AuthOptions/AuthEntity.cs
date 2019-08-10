using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An entity related to authentication and authorization.
    /// </summary>
    [Serializable]
    public class AuthEntity : IAuthEntity
    {
        [NonSerialized]
        private string password;

        /// <inheritdoc/>
        public string Username { get; set; }

        /// <inheritdoc/>
        [IgnoreDataMember]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public IReadOnlyList<string> Roles { get; set; }

        IReadOnlyList<string> IAuthEntity.Roles => Roles ?? new List<string>();
    }
}
