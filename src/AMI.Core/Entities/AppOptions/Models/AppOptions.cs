using System;

namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The application options.
    /// </summary>
    [Serializable]
    public class AppOptions : IAppOptions
    {
        /// <inheritdoc/>
        public int MaxSizeKilobytes { get; set; }

        /// <inheritdoc/>
        public int MaxCompressedEntries { get; set; }

        /// <inheritdoc/>
        public int TimeoutMilliseconds { get; set; }

        /// <inheritdoc/>
        public string WorkingDirectory { get; set; }
    }
}
