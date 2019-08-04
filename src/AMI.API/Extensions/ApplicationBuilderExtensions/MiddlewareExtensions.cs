using AMI.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ApplicationBuilderExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Extension method used to add the middleware to the HTTP request pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>A <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }

        /// <summary>
        /// Uses the throttle middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The application builder using the throttle middleware.</returns>
        public static IApplicationBuilder UseThrottleMiddleware(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            return builder.UseMiddleware<ThrottleMiddleware>();
        }
    }
}
