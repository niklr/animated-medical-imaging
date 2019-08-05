using System;
using System.IO;
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

            return builder.UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    IAppOptions appOptions = new AppOptions();
                    hostingContext.Configuration.GetSection("AppOptions").Bind(appOptions);
                    if (string.IsNullOrWhiteSpace(appOptions.WorkingDirectory))
                    {
                        throw new UnexpectedNullException("Working directory is not defined.");
                    }

                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                    if (bool.TryParse(hostingContext.Configuration["Logging:WriteToFile"], out bool writeToFile))
                    {
                        if (writeToFile)
                        {
                            var logPath = Path.Combine(appOptions.WorkingDirectory, "Logs");
                            Directory.CreateDirectory(logPath);
                            loggerConfiguration
                            .WriteTo.File(
                                new RenderedCompactJsonFormatter(),
                                Path.Combine(logPath, "AMI.API.log.txt"),
                                fileSizeLimitBytes: 1_000_000,
                                rollOnFileSizeLimit: true,
                                retainedFileCountLimit: 31,
                                shared: true,
                                flushToDiskInterval: TimeSpan.FromSeconds(5));
                        }
                    }
                });
        }
    }
}
