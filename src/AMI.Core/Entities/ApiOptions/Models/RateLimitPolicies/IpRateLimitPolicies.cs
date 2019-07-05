using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The policies used to limit the rate base on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class IpRateLimitPolicies : IIpRateLimitPolicies
    {
        /// <summary>
        /// Gets or sets the rules to limit the rate based on the IP address of the client.
        /// </summary>
        public IReadOnlyList<IpRateLimitPolicy> IpRules { get; set; }

        IReadOnlyList<IIpRateLimitPolicy> IIpRateLimitPolicies.IpRules => IpRules ?? new List<IpRateLimitPolicy>();
    }
}
