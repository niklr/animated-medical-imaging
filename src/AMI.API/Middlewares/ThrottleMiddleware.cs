using System;
using AMI.API.Extensions.HttpContextExtensions;
using AMI.Core.Configurations;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMI.API.Middlewares
{
    /// <summary>
    /// The middleware to throttle requests based on the IP address of the client.
    /// </summary>
    /// <seealso cref="IpRateLimitMiddleware" />
    public class ThrottleMiddleware : IpRateLimitMiddleware
    {
        private readonly IApiConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="options">The options.</param>
        /// <param name="counterStore">The counter store.</param>
        /// <param name="policyStore">The policy store.</param>
        /// <param name="config">The rate limit configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The API configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public ThrottleMiddleware(
            RequestDelegate next,
            IOptions<IpRateLimitOptions> options,
            IRateLimitCounterStore counterStore,
            IIpPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<IpRateLimitMiddleware> logger,
            IApiConfiguration configuration)
            : base(next, options, counterStore, policyStore, config, logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public override ClientRequestIdentity ResolveIdentity(HttpContext httpContext)
        {
            return new ClientRequestIdentity
            {
                ClientIp = httpContext?.GetRemoteIpAddress(configuration),
                Path = httpContext?.Request?.Path.ToString().ToLowerInvariant(),
                HttpVerb = httpContext?.Request?.Method?.ToLowerInvariant(),
                ClientId = string.Empty
            };
        }
    }
}
