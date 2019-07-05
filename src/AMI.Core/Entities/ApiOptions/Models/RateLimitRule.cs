using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The rule to limit the rate.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    [Serializable]
    public class RateLimitRule : IRateLimitRule
    {
        /// <inheritdoc/>
        public string Endpoint { get; set; }

        /// <inheritdoc/>
        public string Period { get; set; }

        /// <inheritdoc/>
        public TimeSpan? PeriodTimespan { get; set; }

        /// <inheritdoc/>
        public double Limit { get; set; }
    }
}
