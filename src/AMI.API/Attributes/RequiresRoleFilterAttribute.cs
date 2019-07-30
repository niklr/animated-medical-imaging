using System;
using AMI.API.Requirements;
using AMI.Core.Providers;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AMI.API.Attributes
{
    /// <summary>
    /// A filter that confirms role-based request authorization.
    /// </summary>
    /// <seealso cref="IAuthorizationFilter" />
    public class RequiresRoleFilterAttribute : IAuthorizationFilter
    {
        private readonly ILogger logger;
        private readonly RoleAuthorizationRequirement requirement;
        private readonly ICustomPrincipalProvider principalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresRoleFilterAttribute"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="requirement">The requirement.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public RequiresRoleFilterAttribute(ILogger<RequiresRoleAttribute> logger, RoleAuthorizationRequirement requirement, ICustomPrincipalProvider principalProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.requirement = requirement ?? throw new ArgumentNullException(nameof(requirement));
            this.principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
        }

        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = false;

            var principal = principalProvider.GetPrincipal();
            if (principal == null)
            {
                throw new UnexpectedNullException("Principal could not be retrieved.");
            }

            foreach (var role in requirement.RequiredRoles)
            {
                if (principal.IsInRole(role.ToString()))
                {
                    isAuthorized = true;
                    break;
                }
            }

            if (!isAuthorized)
            {
                string message = string.Concat("One or more roles are missing: ", string.Join(", ", requirement.RequiredRoles));
                logger.LogInformation(message);
                throw new ForbiddenException(message);
            }
        }
    }
}
