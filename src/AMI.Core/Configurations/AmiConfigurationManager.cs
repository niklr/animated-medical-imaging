using System;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace AMI.Core.Configurations
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
        /// <exception cref="ArgumentNullException">configuration</exception>
        /// <exception cref="UnexpectedNullException">configuration - AppSettings</exception>
        public AmiConfigurationManager(IOptions<AppSettings> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.Value == null)
            {
                throw new UnexpectedNullException(nameof(configuration), nameof(AppSettings));
            }

            MaxSizeKilobytes = configuration.Value.MaxSizeKilobytes;
            MaxCompressedEntries = configuration.Value.MaxCompressedEntries;
            TimeoutMilliseconds = configuration.Value.TimeoutMilliseconds;
            WorkingDirectory = configuration.Value.WorkingDirectory;
        }

        /// <inheritdoc/>
        public int MaxSizeKilobytes { get; private set; }

        /// <inheritdoc/>
        public int MaxCompressedEntries { get; private set; }

        /// <inheritdoc/>
        public int TimeoutMilliseconds { get; private set; }

        /// <inheritdoc/>
        public string WorkingDirectory { get; private set; }

        /// <inheritdoc/>
        public AppSettings ToModel()
        {
            return new AppSettings()
            {
                MaxSizeKilobytes = MaxSizeKilobytes,
                MaxCompressedEntries = MaxCompressedEntries,
                TimeoutMilliseconds = TimeoutMilliseconds,
                WorkingDirectory = WorkingDirectory
            };
        }
    }
}
