using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The policy to limit the rate based on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class IpRateLimitPolicy : RateLimitPolicy, IIpRateLimitPolicy
    {
        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        public string Ip { get; set; }
    }
}
