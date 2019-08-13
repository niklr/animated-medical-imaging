using System;
using System.Threading.Tasks;
using AMI.API.Extensions.HttpContextExtensions;
using AMI.Core.Configurations;
using AMI.Core.IO.Serializers;
using AMI.Domain.Exceptions;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RNS.Framework.Tools;

namespace AMI.API.Middlewares
{
    /// <summary>
    /// The middleware to throttle requests based on the IP address of the client.
    /// </summary>
    /// <seealso cref="IpRateLimitMiddleware" />
    public class ThrottleMiddleware : IpRateLimitMiddleware
    {
        private readonly IpRateLimitOptions options;
        private readonly IApiConfiguration configuration;
        private readonly IDefaultJsonSerializer serializer;

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
        /// <param name="serializer">The JSON serializer.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public ThrottleMiddleware(
            RequestDelegate next,
            IOptions<IpRateLimitOptions> options,
            IRateLimitCounterStore counterStore,
            IIpPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<IpRateLimitMiddleware> logger,
            IApiConfiguration configuration,
            IDefaultJsonSerializer serializer)
            : base(next, options, counterStore, policyStore, config, logger)
        {
            Ensure.ArgumentNotNull(options, nameof(options));

            if (options.Value == null)
            {
                throw new UnexpectedNullException("The options value is null.");
            }

            this.options = options.Value;
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc/>
        public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
        {
            var message = string.IsNullOrEmpty(options.QuotaExceededMessage)
                ? $"API calls quota exceeded! Try again in {retryAfter} seconds. Maximum admitted {rule.Limit} per {rule.Period}." : options.QuotaExceededMessage;

            if (!options.DisableRateLimitHeaders)
            {
                httpContext.Response.Headers["Retry-After"] = retryAfter;
            }

            var result = new Core.Entities.Models.ErrorModel()
            {
                Error = message
            };

            httpContext.Response.ContentType = serializer.ContentType;
            httpContext.Response.StatusCode = options.HttpStatusCode;

            return httpContext.Response.WriteAsync(serializer.Serialize(result));
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
