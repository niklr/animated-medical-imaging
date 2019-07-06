using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace AMI.API.Providers
{
    /// <summary>
    /// A custom SignalR provider to configure the "User ID" of a connection.
    /// </summary>
    public class CustomUserIdProvider : IUserIdProvider
    {
        /// <inheritdoc/>
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
