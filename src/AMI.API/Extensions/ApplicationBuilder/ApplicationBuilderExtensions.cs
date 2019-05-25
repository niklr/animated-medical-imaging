using System;
using AMI.API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AMI.API.Extensions.ApplicationBuilderExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Extension method used to add the middleware to the HTTP request pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>A <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
