using AMI.Core.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class CustomOptionsExtensions
    {
        /// <summary>
        /// Extension method used to add the custom options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            Ensure.ArgumentNotNull(services, nameof(services));
            Ensure.ArgumentNotNull(configuration, nameof(configuration));

            services.AddOptions();
            services.Configure<AppOptions>(configuration.GetSection("AppOptions"));
            services.Configure<ApiOptions>(configuration.GetSection("ApiOptions"));
            services.Configure<AspNetCoreRateLimit.IpRateLimitOptions>(configuration.GetSection("ApiOptions:IpRateLimiting"));
            services.Configure<AspNetCoreRateLimit.IpRateLimitPolicies>(configuration.GetSection("ApiOptions:IpRateLimitPolicies"));
        }
    }
}
