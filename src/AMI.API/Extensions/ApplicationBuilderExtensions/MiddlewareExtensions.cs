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
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            builder.UseMiddleware<CustomExceptionMiddleware>();
        }

        /// <summary>
        /// Extension method used to add the throttle middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void UseThrottleMiddleware(this IApplicationBuilder builder)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));

            builder.UseMiddleware<ThrottleMiddleware>();
        }
    }
}
