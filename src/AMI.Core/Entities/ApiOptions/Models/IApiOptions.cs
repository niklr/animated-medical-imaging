namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the API options.
    /// </summary>
    public interface IApiOptions
    {
        /// <summary>
        /// Gets the cleanup period in minutes. Default is 0 to prevent any cleanup.
        /// Automatically deletes objects older than the defined period.
        /// </summary>
        int CleanupPeriod { get; }

        /// <summary>
        /// Gets the name of header used to identify the IP address of the connecting client.
        /// </summary>
        string ConnectingIpHeaderName { get; }

        /// <summary>
        /// Gets a value indicating whether the current environment is development.
        /// </summary>
        bool IsDevelopment { get; }

        /// <summary>
        /// Gets the options used for authentication and authorization.
        /// </summary>
        IAuthOptions AuthOptions { get; }

        /// <summary>
        /// Gets the options used to limit the rate based on the IP address of the client.
        /// </summary>
        IIpRateLimitOptions IpRateLimiting { get; }

        /// <summary>
        /// Gets the policies used to limit the rate base on the IP address of the client.
        /// </summary>
        IIpRateLimitPolicies IpRateLimitPolicies { get; }
    }
}
