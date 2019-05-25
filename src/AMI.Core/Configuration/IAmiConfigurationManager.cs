namespace AMI.Core.Configuration
{
    /// <summary>
    /// A manager for the application configuration.
    /// </summary>
    public interface IAmiConfigurationManager
    {
        /// <summary>
        /// Gets a value indicating whether the current environment is development.
        /// </summary>
        bool IsDevelopment { get; }

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

        /// <summary>
        /// Gets the working directory.
        /// </summary>
        string WorkingDirectory { get; }
    }
}
