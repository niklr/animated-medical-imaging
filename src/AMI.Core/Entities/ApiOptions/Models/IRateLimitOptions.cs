using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the options used to limit the rate.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IRateLimitOptions
    {
        /// <summary>
        /// Gets the general rules to limit the rate.
        /// </summary>
        IReadOnlyList<IRateLimitRule> GeneralRules { get; }

        /// <summary>
        /// Gets the endpoint whitelist.
        /// </summary>
        IReadOnlyList<string> EndpointWhitelist { get; }

        /// <summary>
        /// Gets the client whitelist.
        /// </summary>
        IReadOnlyList<string> ClientWhitelist { get; }

        /// <summary>
        /// Gets the HTTP Status code returned when rate limiting occurs, by default value is set to 429 (Too Many Requests).
        /// </summary>
        int HttpStatusCode { get; }

        /// <summary>
        /// Gets a value that will be used as a formatter for the QuotaExceeded response message.
        /// If none specified the default will be: API calls quota exceeded! maximum admitted {0} per {1}
        /// </summary>
        string QuotaExceededMessage { get; }

        /// <summary>
        /// Gets a model that represents the QuotaExceeded response (content-type, content, status code).
        /// </summary>
        IQuotaExceededResponse QuotaExceededResponse { get; }

        /// <summary>
        /// Gets the counter prefix, used to compose the rate limit counter cache key
        /// </summary>
        string RateLimitCounterPrefix { get; }

        /// <summary>
        /// Gets a value indicating whether all requests, including the rejected ones, should be stacked in this order: day, hour, min, sec
        /// </summary>
        bool StackBlockedRequests { get; }

        /// <summary>
        /// Gets a value indicating whether endpoint rate limiting based URL path and HTTP verb is enabled.
        /// </summary>
        bool EnableEndpointRateLimiting { get; }

        /// <summary>
        /// Gets a value indicating whether X-Rate-Limit and Rety-After headers are disabled.
        /// </summary>
        bool DisableRateLimitHeaders { get; }
    }
}
