using System;
using System.Threading.Tasks;
using AMI.Core.Services;

namespace AMI.API.Hubs
{
    /// <summary>
    /// The gateway SignalR hub.
    /// </summary>
    public class GatewayHub : BaseHub
    {
        private readonly IGatewayService gateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayHub"/> class.
        /// </summary>
        /// <param name="gateway">The gateway service.</param>
        /// <exception cref="ArgumentNullException">gateway</exception>
        public GatewayHub(IGatewayService gateway)
        {
            this.gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        }

        /// <inheritdoc/>
        public override async Task OnConnectedAsync()
        {
            // TODO: limit max number of connections per user
            foreach (var groupName in gateway.GetGroupNames(Context.UserIdentifier))
            {
                await AddToGroup(groupName);
            }

            await base.OnConnectedAsync();
        }

        /// <inheritdoc/>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var groupName in gateway.GetGroupNames(Context.UserIdentifier))
            {
                await RemoveFromGroup(groupName);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
