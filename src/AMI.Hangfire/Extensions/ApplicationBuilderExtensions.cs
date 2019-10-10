using Hangfire;
using Microsoft.AspNetCore.Builder;
using RNS.Framework.Tools;

namespace AMI.Hangfire.Extensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Extension method used to add the dashboard UI of Hangfire.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>A <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseCustomHangfireDashboard(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            return builder.UseHangfireDashboard();
        }
    }
}
