using System.Threading.Tasks;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a service to handle background processing.
    /// </summary>
    public interface IBackgroundService
    {
        /// <summary>
        /// Enqueues the task asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> EnqueueTaskAsync(string id);

        /// <summary>
        /// Schedules the recurring cleanup job.
        /// </summary>
        void ScheduleCleanup();
    }
}
