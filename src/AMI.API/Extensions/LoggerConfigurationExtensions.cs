using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
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
        /// Configures the minimum level at which events will be passed to sinks
        /// based on the configuration section Logging:LogLevel:Default. If not specified,
        /// only events at the Information level and above will be passed through.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The configuration object allowing method chaining.</returns>
        public static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var section = configuration.GetSection("Logging:LogLevel:Default");
            if (section == null || string.IsNullOrWhiteSpace(section.Value))
            {
                loggerConfiguration.MinimumLevel.Information();
            }
            else
            {
                switch (section.Value)
                {
                    case "Trace":
                        loggerConfiguration.MinimumLevel.Verbose();
                        break;
                    case "Debug":
                        loggerConfiguration.MinimumLevel.Debug();
                        break;
                    case "Warning":
                        loggerConfiguration.MinimumLevel.Warning();
                        break;
                    case "Error":
                        loggerConfiguration.MinimumLevel.Error();
                        break;
                    case "Critical":
                        loggerConfiguration.MinimumLevel.Fatal();
                        break;
                    default:
                        loggerConfiguration.MinimumLevel.Information();
                        break;
                }
            }

            return loggerConfiguration;
        }

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