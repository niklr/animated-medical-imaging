using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The options to limit the rate based on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class IpRateLimitOptions : RateLimitOptions, IIpRateLimitOptions
    {
        /// <inheritdoc/>
        public string RealIpHeader { get; set; } = "X-Real-IP";

        /// <inheritdoc/>
        public string ClientIdHeader { get; set; } = "X-ClientId";

        /// <inheritdoc/>
        public string IpPolicyPrefix { get; set; } = "ippp";

        /// <summary>
        /// Gets or sets the IP address whitelist.
        /// </summary>
        public IReadOnlyList<string> IpWhitelist { get; set; }

        IReadOnlyList<string> IIpRateLimitOptions.IpWhitelist => IpWhitelist ?? new List<string>();
    }
}
