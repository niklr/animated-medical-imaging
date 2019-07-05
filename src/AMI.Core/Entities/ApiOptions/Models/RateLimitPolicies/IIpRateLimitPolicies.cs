using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the policies used to limit the rate base on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IIpRateLimitPolicies
    {
        /// <summary>
        /// Gets the rules to limit the rate based on the IP address of the client.
        /// </summary>
        IReadOnlyList<IIpRateLimitPolicy> IpRules { get; }
    }
}
