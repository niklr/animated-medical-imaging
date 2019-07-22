using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The options used for authentication and authorization.
    /// </summary>
    /// <seealso cref="IAuthOptions" />
    [Serializable]
    public class AuthOptions : IAuthOptions
    {
        [NonSerialized]
        private IReadOnlyList<AuthEntity> entities;

        /// <inheritdoc/>
        public bool AllowAnonymous { get; set; }

        /// <inheritdoc/>
        public string AnonymousUsername { get; set; } = "Anon";

        /// <inheritdoc/>
        public int ExpireAfter { get; set; } = 60;

        /// <inheritdoc/>
        [IgnoreDataMember]
        public IAuthJwtOptions JwtOptions { get; set; } = new AuthJwtOptions();

        /// <summary>
        /// Gets or sets the entities allowed to authenticate.
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyList<AuthEntity> Entities
        {
            get
            {
                return entities;
            }

            set
            {
                entities = value;
            }
        }

        [IgnoreDataMember]
        IReadOnlyList<IAuthEntity> IAuthOptions.Entities => Entities ?? new List<AuthEntity>();
    }
}
