using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;

namespace AMI.Hangfire.Filters
{
    /// <summary>
    /// A filter to authorize requests against the Hangfire dashboard.
    /// </summary>
    /// <seealso cref="IDashboardAuthorizationFilter" />
    public class CustomHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Authorizes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var result = httpContext.AuthenticateAsync("Cookies").Result;
            return result?.Principal?.IsInRole("Administrator") ?? false;
        }
    }
}
