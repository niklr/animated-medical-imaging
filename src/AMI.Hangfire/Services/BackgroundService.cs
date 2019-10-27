using System;
using AMI.Core.Configurations;
using AMI.Core.Constants;
using AMI.Core.Services;
using Hangfire;

namespace AMI.Hangfire.Services
{
    /// <summary>
    /// A service to handle background processing.
    /// </summary>
    public class BackgroundService : IBackgroundService
    {
        private readonly IApiConfiguration configuration;
        private readonly IBackgroundJobClient client;
        private readonly IRecurringJobManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundService"/> class.
        /// </summary>
        /// <param name="configuration">The API configuration.</param>
        /// <param name="client">The background job client.</param>
        /// <param name="manager">The recurring job manager.</param>
        public BackgroundService(
            IApiConfiguration configuration,
            IBackgroundJobClient client,
            IRecurringJobManager manager)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        /// <inheritdoc/>
        public string EnqueueTask(string id)
        {
            return client.Enqueue<ITaskService>(x => x.ProcessAsync(id, JobCancellationToken.Null));
        }

        /// <inheritdoc/>
        public void ScheduleCleanup()
        {
            string recurringJobId = "cleanup";

            if (configuration.Options.CleanupPeriod > 0)
            {
                manager.AddOrUpdate<ICleanupService>(
                    recurringJobId,
                    x => x.CleanupAsync(JobCancellationToken.Null),
                    "0 * * ? * *", // Every minute
                    TimeZoneInfo.Utc,
                    QueueNames.Default);
            }
            else
            {
                manager.RemoveIfExists(recurringJobId);
            }
        }
    }
}
