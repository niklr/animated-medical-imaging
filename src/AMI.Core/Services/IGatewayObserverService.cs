using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Observers;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a gateway observer service.
    /// </summary>
    public interface IGatewayObserverService
    {
        /// <summary>
        /// Adds the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        void Add(IGatewayObserver observer);

        /// <summary>
        /// Notifies the specified group asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task NotifyAsync<T>(string groupName, T data, CancellationToken ct);

        /// <summary>
        /// Removes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        void Remove(IGatewayObserver observer);
    }
}
