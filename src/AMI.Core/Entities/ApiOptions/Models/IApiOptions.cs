namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the API options.
    /// </summary>
    public interface IApiOptions
    {
        /// <summary>
        /// Gets the name of header used to identify the IP address of the connecting client.
        /// </summary>
        string ConnectingIpHeaderName { get; }

        /// <summary>
        /// Gets a value indicating whether the current environment is development.
        /// </summary>
        bool IsDevelopment { get; }

        /// <summary>
        /// Gets the options used to limit the rate based on the IP address of the client.
        /// </summary>
        IIpRateLimitOptions IpRateLimiting { get; }
    }
}
