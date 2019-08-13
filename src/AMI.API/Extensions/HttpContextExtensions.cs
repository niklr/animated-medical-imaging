using System;
using System.Linq;
using AMI.Core.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.HttpContextExtensions
{
    /// <summary>
    /// Extensions related to <see cref="HttpContext"/>
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
            Ensure.ArgumentNotNull(context, nameof(context));
            Ensure.ArgumentNotNull(configuration, nameof(configuration));

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
