using System;
using System.Collections.Generic;
using System.Security.Principal;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Entities;
using AMI.Domain.Enums;
using RNS.Framework.Tools;

namespace AMI.Core.Entities.Users.Models
{
    /// <summary>
    /// An abstraction that encapsulates an identity and roles.
    /// </summary>
    public class EntityPrincipal : ICustomPrincipal
    {
        private readonly HashSet<string> roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPrincipal"/> class.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <param name="constants">The application constants.</param>
        public EntityPrincipal(UserEntity entity, IApplicationConstants constants)
        {
            Ensure.ArgumentNotNull(entity, nameof(entity));
            Ensure.ArgumentNotNull(constants, nameof(constants));

            Identity = new CustomIdentity()
            {
                Name = entity.Id.ToString()
            };

            roles = string.IsNullOrWhiteSpace(entity.Roles) ?
                    new HashSet<string>() :
                    new HashSet<string>(entity.Roles.Replace(constants.RoleNameSeparator, string.Empty).Split(','));
        }

        /// <inheritdoc/>
        public ICustomIdentity Identity { get; }

        IIdentity IPrincipal.Identity => Identity;

        /// <inheritdoc/>
        public string IpAddress { get; }

        /// <inheritdoc/>
        public bool IsInRole(RoleType role)
        {
            return roles.Contains(role.ToString());
        }

        /// <inheritdoc/>
        public bool IsInRole(string role)
        {
            if (Enum.TryParse(role, out RoleType roleType))
            {
                return IsInRole(roleType);
            }

            return false;
        }
    }
}
