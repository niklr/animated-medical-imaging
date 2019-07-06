using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Observers;
using AMI.Core.Services;

namespace AMI.Infrastructure.Services
{
    /// <summary>
    /// The gateway observer service.
    /// </summary>
    public class GatewayObserverService : IGatewayObserverService
    {
        private readonly IList<IGatewayObserver> observers = new List<IGatewayObserver>();

        /// <inheritdoc/>
        public void Add(IGatewayObserver observer)
        {
            observers.Add(observer);
        }

        /// <inheritdoc/>
        public Task NotifyAsync<T>(string groupName, T data, CancellationToken ct)
        {
            return Task.Run(() => Parallel.ForEach(observers, observer =>
            {
                observer.NotifyAsync(groupName, data, ct);
            }));
        }

        /// <inheritdoc/>
        public void Remove(IGatewayObserver observer)
        {
            observers.Remove(observer);
        }
    }
}
