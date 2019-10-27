using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMI.Core.IO.Builders;
using AMI.Domain.Enums;

namespace AMI.Core.Services
{
    /// <summary>
    /// An interface representing a gateway service.
    /// </summary>
    public interface IGatewayService
    {
        /// <summary>
        /// Gets the builder for gateway group names.
        /// </summary>
        IGatewayGroupNameBuilder Builder { get; }

        /// <summary>
        /// Gets the group names of the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The group names of the specified user.</returns>
        IEnumerable<string> GetGroupNames(string userId);

        /// <summary>
        /// Notifies the group asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="userId">The user identifier.</param>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task NotifyGroupsAsync<T>(string userId, EventType eventType, T data, CancellationToken ct);
    }
}
