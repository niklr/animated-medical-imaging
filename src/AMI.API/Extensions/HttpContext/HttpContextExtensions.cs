using System.Linq;
using AMI.Core.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AMI.API.Extensions.HttpContextExtensions
{
    /// <summary>
    /// Extensions related to the HTTP context.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the remote IP address.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The remote IP address.</returns>
        public static string GetRemoteIpAddress(this HttpContext context, IApiConfiguration configuration)
        {
            string remoteIp = null;

            if (!string.IsNullOrWhiteSpace(configuration.Options.ConnectingIpHeaderName))
            {
                if (context?.Request?.Headers?.TryGetValue(configuration.Options.ConnectingIpHeaderName, out StringValues connectingIpHeader) ?? false)
                {
                    remoteIp = connectingIpHeader.FirstOrDefault();
                }
            }

            if (string.IsNullOrWhiteSpace(remoteIp))
            {
                remoteIp = context?.Connection?.RemoteIpAddress?.ToString();
            }

            return remoteIp;
        }
    }
}
