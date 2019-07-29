using AMI.Core.IO.Extractors;
using AMI.Itk.Extractors;
using AMI.Itk.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Itk.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add the default ITK services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDefaultItk(this IServiceCollection services)
        {
            services.AddScoped<IImageExtractor, ItkImageExtractor>();
            services.AddSingleton<IItkImageReaderFactory, ItkImageReaderFactory>();
        }
    }
}
