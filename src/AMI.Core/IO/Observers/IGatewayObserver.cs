using System.Threading;
using System.Threading.Tasks;

namespace AMI.Core.IO.Observers
{
    /// <summary>
    /// An interface representing a gateway observer.
    /// </summary>
    public interface IGatewayObserver
    {
        /// <summary>
        /// Notifies the the specified group asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task NotifyAsync<T>(string groupName, T data, CancellationToken ct);
    }
}
