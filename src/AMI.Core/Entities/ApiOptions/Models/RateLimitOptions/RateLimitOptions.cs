using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The options used to limit the rate.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class RateLimitOptions : IRateLimitOptions
    {
        /// <summary>
        /// Gets or sets the general rules to limit the rate.
        /// </summary>
        public IReadOnlyList<RateLimitRule> GeneralRules { get; set; }

        /// <summary>
        /// Gets or sets the endpoint whitelist.
        /// </summary>
        public IReadOnlyList<string> EndpointWhitelist { get; set; }

        /// <summary>
        /// Gets or sets the client whitelist.
        /// </summary>
        public IReadOnlyList<string> ClientWhitelist { get; set; }

        /// <inheritdoc/>
        public int HttpStatusCode { get; set; } = 429;

        /// <inheritdoc/>
        public string QuotaExceededMessage { get; set; }

        /// <inheritdoc/>
        public IQuotaExceededResponse QuotaExceededResponse { get; set; } = new QuotaExceededResponse();

        /// <inheritdoc/>
        public string RateLimitCounterPrefix { get; set; } = "crlc";

        /// <inheritdoc/>
        public bool StackBlockedRequests { get; set; }

        /// <inheritdoc/>
        public bool EnableEndpointRateLimiting { get; set; }

        /// <inheritdoc/>
        public bool DisableRateLimitHeaders { get; set; }

        IReadOnlyList<IRateLimitRule> IRateLimitOptions.GeneralRules => GeneralRules ?? new List<RateLimitRule>();

        IReadOnlyList<string> IRateLimitOptions.EndpointWhitelist => EndpointWhitelist ?? new List<string>();

        IReadOnlyList<string> IRateLimitOptions.ClientWhitelist => ClientWhitelist ?? new List<string>();
    }
}
