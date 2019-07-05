namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the policy to limit the rate based on the IP address of the client.
    /// Source: https://github.com/stefanprodan/AspNetCoreRateLimit
    /// </summary>
    public interface IIpRateLimitPolicy : IRateLimitPolicy
    {
        /// <summary>
        /// Gets the IP address.
        /// </summary>
        string Ip { get; }
    }
}
