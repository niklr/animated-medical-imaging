using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the options to limit the rate based on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IIpRateLimitOptions : IRateLimitOptions
    {
        /// <summary>
        /// Gets the HTTP header of the real ip header injected by reverse proxy, by default is X-Real-IP
        /// </summary>
        string RealIpHeader { get; }

        /// <summary>
        /// Gets the HTTP header that holds the client identifier, by default is X-ClientId
        /// </summary>
        string ClientIdHeader { get; }

        /// <summary>
        /// Gets the policy prefix, used to compose the client policy cache key
        /// </summary>
        string IpPolicyPrefix { get; }

        /// <summary>
        /// Gets the IP address whitelist.
        /// </summary>
        IReadOnlyList<string> IpWhitelist { get; }
    }
}
