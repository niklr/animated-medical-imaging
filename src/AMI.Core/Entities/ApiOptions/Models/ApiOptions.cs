using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The API options.
    /// </summary>
    [Serializable]
    public class ApiOptions : IApiOptions
    {
        /// <inheritdoc/>
        public int BatchSize { get; set; } = 1000;

        /// <inheritdoc/>
        public int CleanupPeriod { get; set; } = 0;

        /// <inheritdoc/>
        public string ConnectingIpHeaderName { get; set; }

        /// <inheritdoc/>
        public bool IsDevelopment { get; set; } = false;

        /// <inheritdoc/>
        public int RequestTimeoutMilliseconds { get; set; } = 5000;

        /// <inheritdoc/>
        public IAuthOptions AuthOptions { get; set; } = new AuthOptions();

        /// <inheritdoc/>
        public IIpRateLimitOptions IpRateLimiting { get; set; } = new IpRateLimitOptions();

        /// <inheritdoc/>
        public IIpRateLimitPolicies IpRateLimitPolicies { get; set; } = new IpRateLimitPolicies();
    }
}
