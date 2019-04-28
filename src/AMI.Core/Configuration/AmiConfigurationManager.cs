using System.Configuration;

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
        public AmiConfigurationManager()
        {
            MaxSizeKilobytes = int.TryParse(ConfigurationManager.AppSettings["AMI.MaxSizeKilobytes"], out int maxSizeKilobytes) ? maxSizeKilobytes : 0;
            MaxCompressedEntries = int.TryParse(ConfigurationManager.AppSettings["AMI.MaxCompressedEntries"], out int maxCompressedEntries) ? maxCompressedEntries : 0;
            TimeoutMilliseconds = int.TryParse(ConfigurationManager.AppSettings["AMI.TimeoutMilliseconds"], out int timeoutMilliseconds) ? timeoutMilliseconds : 0;
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
    }
}
