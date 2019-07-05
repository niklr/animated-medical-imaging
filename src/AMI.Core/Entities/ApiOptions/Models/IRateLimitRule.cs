using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the rule to limit the rate.
    /// </summary>
    public interface IRateLimitRule
    {
        /// <summary>
        /// Gets the HTTP verb and path.
        /// </summary>
        /// <example>
        /// get:/api/values
        /// *:/api/values
        /// *
        /// </example>
        string Endpoint { get; }

        /// <summary>
        /// Gets the rate limit period as in 1s, 1m, 1h.
        /// </summary>
        string Period { get; }

        /// <summary>
        /// Gets the rate limit period as time interval.
        /// </summary>
        TimeSpan? PeriodTimespan { get; }

        /// <summary>
        /// Gets the maximum number of requests that a client can make in a defined period.
        /// </summary>
        double Limit { get; }
    }
}
