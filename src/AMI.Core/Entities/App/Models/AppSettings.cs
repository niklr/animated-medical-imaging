namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// The application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the current environment is development.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        /// Gets or sets the maximum size in kilobytes.
        /// </summary>
        public int MaxSizeKilobytes { get; set; }

        /// <summary>
        /// Gets or sets the maximum of compressed entries.
        /// </summary>
        public int MaxCompressedEntries { get; set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }
    }
}
