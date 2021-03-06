﻿using System;
using System.Runtime.Serialization;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The JSON Web Token (JWT) options.
    /// </summary>
    /// <seealso cref="IAuthJwtOptions" />
    [Serializable]
    public class AuthJwtOptions : IAuthJwtOptions
    {
        [NonSerialized]
        private string secretKey;

        /// <inheritdoc/>
        [IgnoreDataMember]
        public string SecretKey
        {
            get
            {
                return secretKey;
            }

            set
            {
                secretKey = value;
            }
        }

        /// <inheritdoc/>
        public string Issuer { get; set; }

        /// <inheritdoc/>
        public string Audience { get; set; }

        /// <inheritdoc/>
        public string NameClaimType { get; set; } = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        /// <inheritdoc/>
        /// <remarks>Roles are not mapped correctly if RoleClaimType = "roles"</remarks>
        public string RoleClaimType { get; set; } = "roleClaims";

        /// <inheritdoc/>
        public string IssuerClaimType { get; set; } = "iss";

        /// <inheritdoc/>
        public string UsernameClaimType { get; set; } = "username";
    }
}
