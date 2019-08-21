using System;
using AMI.Core.Providers;
using AMI.Core.Services;
using AMI.Domain.Enums;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using RNS.Framework.Tools;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// A service for authorization purposes.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ILogger logger;
        private readonly ICustomPrincipalProvider principalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public AuthService(ILogger<AuthService> logger, ICustomPrincipalProvider principalProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
        }

        /// <inheritdoc/>
        public bool IsAuthorized(string ownerId)
        {
            try
            {
                Ensure.ArgumentNotNullOrWhiteSpace(ownerId, nameof(ownerId));

                var principal = principalProvider.GetPrincipal();
                if (principal == null)
                {
                    throw new UnexpectedNullException("Principal could not be retrieved.");
                }

                if (ownerId.Equals(principal.Identity.Name) || principal.IsInRole(RoleType.Administrator))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return false;
        }
    }
}
