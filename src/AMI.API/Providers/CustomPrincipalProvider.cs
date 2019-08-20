using AMI.API.Extensions.HttpContextExtensions;
using AMI.Core.Configurations;
using AMI.Core.Entities.Models;
using AMI.Core.Providers;
using Microsoft.AspNetCore.Http;
using RNS.Framework.Tools;

namespace AMI.API.Providers
{
    /// <summary>
    /// A provider for custom principals.
    /// </summary>
    public class CustomPrincipalProvider : ICustomPrincipalProvider
    {
        private readonly IHttpContextAccessor accessor;
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPrincipalProvider"/> class.
        /// </summary>
        /// <param name="accessor">The HTTP context accessor.</param>
        /// <param name="configuration">The API configuration.</param>
        public CustomPrincipalProvider(IHttpContextAccessor accessor, IApiConfiguration configuration)
        {
            Ensure.ArgumentNotNull(accessor, nameof(accessor));
            Ensure.ArgumentNotNull(configuration, nameof(configuration));

            this.accessor = accessor;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public ICustomPrincipal GetPrincipal()
        {
            if (accessor.HttpContext == null)
            {
                // If the HttpContext is null the request was initiated by a background worker.
                return new WorkerPrincipal();
            }

            return new CustomPrincipal(configuration.Options?.AuthOptions?.JwtOptions, accessor.HttpContext?.User, accessor.HttpContext?.GetRemoteIpAddress(configuration));
        }
    }
}
