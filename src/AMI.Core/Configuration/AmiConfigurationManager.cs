using AMI.Core.Models;
using Microsoft.Extensions.Options;

namespace AMI.Core.Configuration
{
    /// <summary>
    /// A manager for the application configuration.
    /// </summary>
    /// <seealso cref="IAmiConfigurationManager" />
    public class AmiConfigurationManager : IAmiConfigurationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmiConfigurationManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AmiConfigurationManager(IOptions<AppSettings> configuration)
        {
            MaxSizeKilobytes = configuration.Value.MaxSizeKilobytes;
            MaxCompressedEntries = configuration.Value.MaxCompressedEntries;
            TimeoutMilliseconds = configuration.Value.TimeoutMilliseconds;
            WorkingDirectory = configuration.Value.WorkingDirectory;
        }

        /// <summary>
        /// Gets the maximum size in kilobytes.
        /// </summary>
        public int MaxSizeKilobytes { get; private set; }

        /// <summary>
        /// Gets the maximum of compressed entries.
        /// </summary>
        public int MaxCompressedEntries { get; private set; }

        /// <summary>
        /// Gets the timeout in milliseconds.
        /// </summary>
        public int TimeoutMilliseconds { get; private set; }

        /// <summary>
        /// Gets the working directory.
        /// </summary>
        public string WorkingDirectory { get; private set; }
    }
}
