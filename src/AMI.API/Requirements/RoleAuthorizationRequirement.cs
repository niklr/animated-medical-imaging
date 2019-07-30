using System.Collections.Generic;
using AMI.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AMI.API.Requirements
{
    /// <summary>
    /// A requirement for role-based authorization.
    /// </summary>
    /// <seealso cref="IAuthorizationRequirement" />
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizationRequirement"/> class.
        /// </summary>
        /// <param name="requiredRoles">The required roles.</param>
        public RoleAuthorizationRequirement(IEnumerable<RoleType> requiredRoles)
        {
            RequiredRoles = requiredRoles ?? new List<RoleType>();
        }

        /// <summary>
        /// Gets the required roles.
        /// </summary>
        public IEnumerable<RoleType> RequiredRoles { get; } = new List<RoleType>();
    }
}
