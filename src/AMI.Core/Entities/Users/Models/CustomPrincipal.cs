using System.Security.Claims;
using RNS.Framework.Tools;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An abstraction that encapsulates an identity and roles.
    /// </summary>
    public class CustomPrincipal : ICustomPrincipal
    {
        private readonly ClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPrincipal"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="principal">The principal.</param>
        public CustomPrincipal(IAuthJwtOptions options, ClaimsPrincipal principal)
        {
            Ensure.ArgumentNotNull(options, nameof(options));
            Ensure.ArgumentNotNull(principal, nameof(principal));

            this.principal = principal;
            var usernameClaim = principal.FindFirst(options.UsernameClaimType);
            var issuerClaim = principal.FindFirst(options.IssuerClaimType);
            Identity = new CustomIdentity()
            {
                Name = principal.Identity.Name,
                Username = usernameClaim?.Value,
                Domain = issuerClaim?.Value
            };
        }

        /// <inheritdoc/>
        public ICustomIdentity Identity { get; }

        /// <inheritdoc/>
        public bool IsInRole(string role)
        {
            return principal.IsInRole(role);
        }
    }
}
