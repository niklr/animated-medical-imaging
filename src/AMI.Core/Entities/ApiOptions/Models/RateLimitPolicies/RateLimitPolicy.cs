using System;
using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The policy to limit the rate.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class RateLimitPolicy : IRateLimitPolicy
    {
        /// <summary>
        /// Gets or sets the rules to limit the rate.
        /// </summary>
        public IReadOnlyList<RateLimitRule> Rules { get; set; }

        IReadOnlyList<IRateLimitRule> IRateLimitPolicy.Rules => Rules ?? new List<RateLimitRule>();
    }
}
