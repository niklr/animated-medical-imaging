using System;
using System.IO;
using AMI.API.Extensions.LoggerConfigurationExtensions;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RNS.Framework.Tools;
using Serilog;
using Serilog.Formatting.Compact;

namespace AMI.API.Extensions.WebHostBuilderExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IWebHostBuilder"/>
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Appends Serilog as the logging provider.
        /// </summary>
        /// <param name="builder">The web host builder to configure.</param>
        /// <returns>The web host builder.</returns>
        public static IWebHostBuilder AppendSerilog(this IWebHostBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            IApplicationConstants constants = new ApplicationConstants();

            return builder.UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    IAppOptions appOptions = new AppOptions();
                    hostingContext.Configuration.GetSection("AppOptions").Bind(appOptions);
                    if (string.IsNullOrWhiteSpace(appOptions.WorkingDirectory))
                    {
                        throw new UnexpectedNullException("Working directory is not defined.");
                    }

                    loggerConfiguration
                        .SetMinimumLevel(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();

                    if (bool.TryParse(hostingContext.Configuration["Logging:WriteToFile"], out bool writeToFile))
                    {
                        if (writeToFile)
                        {
                            string path = Path.Combine(appOptions.WorkingDirectory, constants.LogFilePath);
                            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var logFilePath))
                            {
                                throw new Exception($"Invalid log file path '{constants.LogFilePath}'.");
                            }

                            var logFile = new FileInfo(logFilePath.LocalPath);
                            logFile.Directory?.Create();
                            loggerConfiguration
                            .WriteTo.File(
                                new RenderedCompactJsonFormatter(),
                                logFile.FullName,
                                fileSizeLimitBytes: 10_000_000,
                                rollOnFileSizeLimit: true,
                                retainedFileCountLimit: 2,
                                shared: true,
                                flushToDiskInterval: TimeSpan.FromSeconds(5));
                        }
                    }
                    if (bool.TryParse(hostingContext.Configuration["Logging:WriteToDb"], out bool writeToDb))
                    {
                        if (writeToDb)
                        {
                            loggerConfiguration
                            .WriteToSqlite(appOptions, constants);
                        }
                    }
                });
        }
    }
}
