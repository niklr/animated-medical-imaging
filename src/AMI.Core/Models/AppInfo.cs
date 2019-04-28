using System;

namespace AMI.Core.Models
{
    /// <summary>
    /// A model containing information about the application.
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppInfo"/> class specifying application name and version.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="version">The version of the application.</param>
        /// <exception cref="ArgumentNullException">appName</exception>
        public AppInfo(string appName, string version = null)
        {
            if (string.IsNullOrWhiteSpace(nameof(appName)))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            AppName = appName;
            AppVersion = version;
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        public string AppName { get; }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        public string AppVersion { get; }
    }
}
