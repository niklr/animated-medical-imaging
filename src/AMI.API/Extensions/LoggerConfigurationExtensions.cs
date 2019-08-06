using System.Globalization;
using System.IO;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using RNS.Framework.Tools;
using Serilog;

namespace AMI.API.Extensions.LoggerConfigurationExtensions
{
    /// <summary>
    /// Extensions related to <see cref="LoggerConfiguration"/>
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Writes log events to SQLite.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="options">The application options.</param>
        /// <param name="constants">The application constants.</param>
        public static void WriteToSqlite(this LoggerConfiguration loggerConfiguration, IAppOptions options, IApplicationConstants constants)
        {
            Ensure.ArgumentNotNull(loggerConfiguration, nameof(loggerConfiguration));
            Ensure.ArgumentNotNull(options, nameof(options));
            Ensure.ArgumentNotNull(constants, nameof(constants));

            if (string.IsNullOrWhiteSpace(options.WorkingDirectory))
            {
                throw new UnexpectedNullException("Working directory is not defined.");
            }

            // loggerConfiguration.WriteTo.SQLite(
            //    sqliteDbPath: Path.Combine(options.WorkingDirectory, constants.SqliteLogDatabaseName),
            //    tableName: "ApplicationLogs",
            //    storeTimestampInUtc: true,
            //    formatProvider: CultureInfo.InvariantCulture);
        }
    }
}