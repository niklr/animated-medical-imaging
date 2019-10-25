using System.Threading.Tasks;
using AMI.Core.Constants;
using AMI.Hangfire.Attributes;
using Hangfire;

namespace AMI.Hangfire.Services
{
    /// <summary>
    /// An interface representing a service to handle tasks.
    /// </summary>
    [LogEverything]
    public interface ITaskService
    {
        /// <summary>
        /// Processes the task asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Queue(QueueNames.Imaging)]
        [LogEverything]
        Task ProcessAsync(string id, IJobCancellationToken ct);
    }
}
