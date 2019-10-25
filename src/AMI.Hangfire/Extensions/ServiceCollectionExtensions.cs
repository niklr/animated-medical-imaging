using System;
using System.IO;
using AMI.Core.Constants;
using AMI.Core.Entities.Models;
using AMI.Core.Services;
using AMI.Domain.Exceptions;
using AMI.Hangfire.Attributes;
using AMI.Hangfire.Services;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Extensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add Hangfire for unit testing.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddTestHangfire(this IServiceCollection services)
        {
            Ensure.ArgumentNotNull(services, nameof(services));

            var backgroundJobClient = new Mock<IBackgroundJobClient>();
            services.AddTransient(provider =>
            {
                return backgroundJobClient.Object;
            });

            var recurringJobManager = new Mock<IRecurringJobManager>();
            services.AddTransient(provider =>
            {
                return recurringJobManager.Object;
            });

            services.AddTransient<ICleanupService, CleanupService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddSingleton<IBackgroundService, BackgroundService>();
        }

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

            services.AddTransient<ICleanupService, CleanupService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddSingleton<IBackgroundService, BackgroundService>();

            var appOptions = new AppOptions();
            configuration.GetSection("AppOptions").Bind(appOptions);

            if (string.IsNullOrWhiteSpace(appOptions.WorkingDirectory))
            {
                throw new UnexpectedNullException(string.Format("AppOptions:{0} is missing.", nameof(appOptions.WorkingDirectory)));
            }

            var dbPath = Path.Combine(appOptions.WorkingDirectory, constants.HangfireLiteDbName);
            services.AddHangfire(x => x.UseLiteDbStorage(dbPath));

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { QueueNames.Default, QueueNames.Imaging, QueueNames.Webhooks };
                options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
                options.WorkerCount = 1;
            });

            GlobalJobFilters.Filters.Add(new LogEverythingAttribute());
        }
    }
}
