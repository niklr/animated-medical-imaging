﻿using System;
using System.Security.Principal;
using AMI.Domain.Enums;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An abstraction that encapsulates an identity and roles.
    /// </summary>
    public class MockPrincipal : ICustomPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockPrincipal"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public MockPrincipal(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Identity = new CustomIdentity()
            {
                Name = name
            };
        }

        /// <inheritdoc/>
        public ICustomIdentity Identity { get; }

        IIdentity IPrincipal.Identity => Identity;

        /// <inheritdoc/>
        public bool IsInRole(RoleType role)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
