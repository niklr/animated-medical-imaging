namespace AMI.Core.Entities.Models
{
    /// <summary>
    /// An interface representing the application options.
    /// </summary>
    public interface IAppOptions
    {
        /// <summary>
        /// Gets the maximum size in kilobytes.
        /// </summary>
        int MaxSizeKilobytes { get; }

        /// <summary>
        /// Gets the maximum allowed amount of archived entries.
        /// </summary>
        int MaxArchivedEntries { get; }

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
