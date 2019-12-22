using System;

namespace AMI.Core.Services
{
    /// <summary>
    /// A mock implementation of the background service.
    /// </summary>
    /// <seealso cref="IBackgroundService" />
    public class MockBackgroundService : IBackgroundService
    {
        /// <inheritdoc/>
        public string EnqueueTask(string id)
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public string EnqueueWebhookEvent(string webhookId, string eventId)
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public void ScheduleCleanup()
        {
        }
    }
}
