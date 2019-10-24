using System.Threading.Tasks;
using Hangfire;

namespace AMI.Hangfire.Services
{
    /// <summary>
    /// An interface representing a service to handle cleanups.
    /// </summary>
    public interface ICleanupService
    {
        /// <summary>
        /// Calls the cleanup asynchronous.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CleanupAsync(IJobCancellationToken ct);
    }
}
