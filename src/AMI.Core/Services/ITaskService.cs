using System.Threading.Tasks;
using AMI.Core.Wrappers;

namespace AMI.Core.Services
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
        Task ProcessAsync(string id, IWrappedJobCancellationToken ct);
    }
}
