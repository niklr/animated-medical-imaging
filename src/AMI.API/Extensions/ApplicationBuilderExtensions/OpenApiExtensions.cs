using AMI.Core.Entities.Models;
using Microsoft.AspNetCore.Builder;
using NSwag.AspNetCore;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ApplicationBuilderExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Uses the OpenAPI/Swagger middlewares.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="appInfo">The application information.</param>
        public static void UseOpenApiMiddlewares(this IApplicationBuilder builder, AppInfoModel appInfo)
        {
            Ensure.ArgumentNotNull(builder, nameof(builder));
            Ensure.ArgumentNotNull(appInfo, nameof(appInfo));

            // https://github.com/RSuter/NSwag/wiki/Assembly-loading#net-core
            builder.UseReDoc(options =>
            {
                options.Path = "/redoc";
                options.DocumentPath = "/specification.json";
            });

            // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
            builder.UseSwaggerUi3(options =>
            {
                options.Path = "/swagger";
                options.SwaggerRoutes.Add(new SwaggerUi3Route($"v{appInfo.AppVersion}", "/specification.json"));
            });
        }
    }
}
