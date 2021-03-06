﻿namespace AMI.Core.Constants
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
        /// Gets the path of the log file.
        /// </summary>
        string LogFilePath { get; }

        /// <summary>
        /// Gets the maximum size of a single upload chunk in bytes.
        /// </summary>
        int MaxUploadChunkSize { get; }

        /// <summary>
        /// Gets the minimum size of a single upload chunk in bytes.
        /// </summary>
        int MinUploadChunkSize { get; }

        /// <summary>
        /// Gets the character used to separate string values.
        /// </summary>
        string ValueSeparator { get; }

        /// <summary>
        /// Gets the character representing a wildcard.
        /// </summary>
        string WildcardCharacter { get; }

        /// <summary>
        /// Gets the name of the SQLite database.
        /// </summary>
        string SqliteDatabaseName { get; }

        /// <summary>
        /// Gets the name of the SQLite database for logs.
        /// </summary>
        string SqliteLogDatabaseName { get; }

        /// <summary>
        /// Gets the name of the LiteDB for Hangfire.
        /// </summary>
        string HangfireLiteDbName { get; }
    }
}
