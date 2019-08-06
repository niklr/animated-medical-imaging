namespace AMI.Core.Constants
{
    /// <summary>
    /// Constants of the application.
    /// </summary>
    public interface IApplicationConstants
    {
        /// <summary>
        /// Gets the short version of the application name.
        /// </summary>
        string ApplicationNameShort { get; }

        /// <summary>
        /// Gets the default file extension.
        /// </summary>
        string DefaultFileExtension { get; }

        /// <summary>
        /// Gets the default pagination limit.
        /// </summary>
        int DefaultPaginationLimit { get; }

        /// <summary>
        /// Gets the allowed values of the limit pagination parameter.
        /// </summary>
        int[] AllowedPaginationLimitValues { get; }

        /// <summary>
        /// Gets the name of the log file.
        /// </summary>
        string LogFilename { get; }

        /// <summary>
        /// Gets the character used to separate the role names.
        /// </summary>
        string RoleNameSeparator { get; }

        /// <summary>
        /// Gets the name of the SQLite database.
        /// </summary>
        string SqliteDatabaseName { get; }

        /// <summary>
        /// Gets the name of the SQLite database for logs.
        /// </summary>
        string SqliteLogDatabaseName { get; }
    }
}
