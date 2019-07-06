using System;
using System.Threading;
using System.Threading.Tasks;
using AMI.API.Hubs;
using AMI.Core.IO.Observers;
using Microsoft.AspNetCore.SignalR;

namespace AMI.API.Observers
{
    /// <summary>
    /// The gateway observer.
    /// </summary>
    public class GatewayObserver : IGatewayObserver
    {
        private IHubContext<GatewayHub> context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayObserver"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public GatewayObserver(IHubContext<GatewayHub> context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public Task NotifyAsync<T>(string groupName, T data, CancellationToken ct)
        {
            return context.Clients.Groups(groupName).SendAsync("notify", data, ct);
        }
    }
}
