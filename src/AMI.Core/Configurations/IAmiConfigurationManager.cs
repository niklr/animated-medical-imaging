using AMI.Core.Entities.Models;

namespace AMI.Core.Configurations
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

        /// <summary>
        /// Gets the working directory.
        /// </summary>
        string WorkingDirectory { get; }

        /// <summary>
        /// Converts the application configuration to a model.
        /// </summary>
        /// <returns>The model represeting the application configuration.</returns>
        AppSettings ToModel();
    }
}
