namespace AMI.Core.Configuration
{
    /// <summary>
    /// A manager for the application configuration.
    /// </summary>
    public interface IAmiConfigurationManager
    {
        /// <summary>
        /// Gets the maximum size in kilobytes.
        /// </summary>
        int MaxSizeKilobytes { get; }

        /// <summary>
        /// Gets the maximum of compressed entries.
        /// </summary>
        int MaxCompressedEntries { get; }

        /// <summary>
        /// Gets the timeout in milliseconds.
        /// </summary>
        int TimeoutMilliseconds { get; }
    }
}
