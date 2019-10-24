using System.Threading.Tasks;
using AMI.Core.Constants;
using Hangfire;

namespace AMI.Hangfire.Services
{
    /// <summary>
    /// An interface representing a service to handle tasks.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Processes the task asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Queue(QueueNames.Imaging)]
        Task ProcessAsync(string id, IJobCancellationToken ct);
    }
}
