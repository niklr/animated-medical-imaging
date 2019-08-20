using System;
using System.Collections.Generic;
using System.Security.Principal;
using AMI.Core.Entities.Models;
using AMI.Domain.Enums;

namespace AMI.NetCore.Tests.Mocks.Core
{
    /// <summary>
    /// An abstraction that encapsulates an identity and roles.
    /// </summary>
    public class MockPrincipal : ICustomPrincipal
    {
        private readonly HashSet<RoleType> roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockPrincipal"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="roles">The roles.</param>
        public MockPrincipal(string name, RoleType[] roles = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Identity = new CustomIdentity()
            {
                Name = name
            };

            this.roles = roles == null ? new HashSet<RoleType>() : new HashSet<RoleType>(roles);
        }

        /// <inheritdoc/>
        public ICustomIdentity Identity { get; }

        IIdentity IPrincipal.Identity => Identity;

        /// <inheritdoc/>
        public string IpAddress { get; }

        /// <inheritdoc/>
        public bool IsInRole(RoleType role)
        {
            return roles.Contains(role);
        }

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
