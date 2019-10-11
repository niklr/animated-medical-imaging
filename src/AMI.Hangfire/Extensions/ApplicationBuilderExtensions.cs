using AMI.Hangfire.Filters;
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
        public static void UseCustomHangfireDashboard(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            var options = new DashboardOptions
            {
                Authorization = new[] { new CustomHangfireAuthorizationFilter() },
                AppPath = "/account/login"
            };

            builder.UseHangfireDashboard("/hangfire", options);
        }
    }
}
