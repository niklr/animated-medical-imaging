using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Domain.Enums;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a gateway service.
    /// </summary>
    public interface IGatewayService
    {
        /// <summary>
        /// Gets the group names relevant to the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The group names relevant to the specified user.</returns>
        IEnumerable<string> GetGroupNames(string userId);

        /// <summary>
        /// Notifies the group asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="gatewayOpCode">The gateway operation code.</param>
        /// <param name="gatewayEvent">The gateway event.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task NotifyGroupAsync<T>(string groupName, GatewayOpCode gatewayOpCode, GatewayEvent gatewayEvent, T data, CancellationToken ct);
    }
}
