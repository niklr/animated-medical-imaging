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
        public int CleanupPeriod { get; set; }

        /// <inheritdoc/>
        public string ConnectingIpHeaderName { get; set; }

        /// <inheritdoc/>
        public bool IsDevelopment { get; set;  }

        /// <inheritdoc/>
        public IIpRateLimitOptions IpRateLimiting { get; set; } = new IpRateLimitOptions();

        /// <inheritdoc/>
        public IIpRateLimitPolicies IpRateLimitPolicies { get; set; } = new IpRateLimitPolicies();
    }
}
