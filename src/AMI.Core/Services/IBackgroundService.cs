namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service to handle background processing.
    /// </summary>
    public interface IBackgroundService
    {
        /// <summary>
        /// Enqueues the task.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>A unique identifier of the created background job.</returns>
        string EnqueueTask(string id);

        /// <summary>
        /// Enqueues the webhook event.
        /// </summary>
        /// <param name="webhookId">The webhook identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>A unique identifier of the created background job.</returns>
        string EnqueueWebhookEvent(string webhookId, string eventId);

        /// <summary>
        /// Schedules the recurring cleanup job.
        /// </summary>
        void ScheduleCleanup();
    }
}
