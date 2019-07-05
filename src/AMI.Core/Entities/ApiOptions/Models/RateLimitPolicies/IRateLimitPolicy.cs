using System.Collections.Generic;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the policy to limit the rate.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IRateLimitPolicy
    {
        /// <summary>
        /// Gets the rules to limit the rate.
        /// </summary>
        IReadOnlyList<IRateLimitRule> Rules { get; }
    }
}
