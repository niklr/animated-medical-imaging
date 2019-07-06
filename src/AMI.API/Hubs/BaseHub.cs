using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AMI.API.Hubs
{
    /// <summary>
    /// The base implementation for SignalR hubs.
    /// </summary>
    public class BaseHub : Hub
    {
        /// <summary>
        /// Adds the current connection to the specified group.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Removes the current connection from the specified group.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
