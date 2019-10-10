using System.IO;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Domain.Exceptions;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Extensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add Hangfire.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="constants">The application constants.</param>
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration, IApplicationConstants constants)
        {
            Ensure.ArgumentNotNull(services, nameof(services));
            Ensure.ArgumentNotNull(configuration, nameof(configuration));
            Ensure.ArgumentNotNull(constants, nameof(constants));

            var appOptions = new AppOptions();
            configuration.GetSection("AppOptions").Bind(appOptions);

            if (string.IsNullOrWhiteSpace(appOptions.WorkingDirectory))
            {
                throw new UnexpectedNullException(string.Format("AppOptions:{0} is missing.", nameof(appOptions.WorkingDirectory)));
            }

            var dbPath = Path.Combine(appOptions.WorkingDirectory, constants.HangfireLiteDbName);
            services.AddHangfire(x => x.UseLiteDbStorage(dbPath));
            services.AddHangfireServer();
        }
    }
}
