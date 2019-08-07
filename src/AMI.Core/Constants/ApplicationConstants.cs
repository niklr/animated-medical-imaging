namespace AMI.Core.Constants
{
    /// <summary>
    /// Constants of the application.
    /// </summary>
    public class ApplicationConstants : IApplicationConstants
    {
        /// <inheritdoc/>
        public string ApplicationNameShort => "AMI";

        /// <inheritdoc/>
        public string DefaultFileExtension => ".ami";

        /// <inheritdoc/>
        public int DefaultPaginationLimit => 25;

        /// <inheritdoc/>
        public int[] AllowedPaginationLimitValues => new int[] { 10, 25, 50 };

        /// <inheritdoc/>
        public string LogFilePath => @"Logs\AMI.log.txt";

        /// <inheritdoc/>
        public string RoleNameSeparator => "#";

        /// <inheritdoc/>
        public string SqliteDatabaseName => "AmiSqliteDatabase.db";

        /// <inheritdoc/>
        public string SqliteLogDatabaseName => "AmiLogSqliteDatabase.db";
    }
}
