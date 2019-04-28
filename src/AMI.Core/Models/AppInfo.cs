using System;
using AMI.Core.Helpers;

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
        /// Gets an empty object used for initialization.
        /// </summary>
        public static AppInfo Empty
        {
            get
            {
                return new AppInfo(string.Empty);
            }
        }

        /// <summary>
        /// Gets the default application information instance.
        /// </summary>
        public static AppInfo Default
        {
            get
            {
                return new AppInfo(ReflectionHelper.GetAssemblyName(), ReflectionHelper.GetAssemblyVersion());
            }
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
