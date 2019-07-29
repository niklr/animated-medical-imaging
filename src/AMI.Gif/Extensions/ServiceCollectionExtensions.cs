using AMI.Core.IO.Writers;
using AMI.Gif.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Gif.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add the default GIF services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDefaultGif(this IServiceCollection services)
        {
            services.AddScoped<IGifImageWriter, AnimatedGifImageWriter>();
        }
    }
}
