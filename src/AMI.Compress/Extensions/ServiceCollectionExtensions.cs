using AMI.Compress.Extractors;
using AMI.Compress.Readers;
using AMI.Compress.Writers;
using AMI.Core.IO.Extractors;
using AMI.Core.IO.Readers;
using AMI.Core.IO.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Compress.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add the default compress services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddDefaultCompress(this IServiceCollection services)
        {
            services.AddScoped<IArchiveReader, SharpCompressReader>();
            services.AddScoped<IArchiveWriter, SharpCompressWriter>();
            services.AddScoped<IArchiveExtractor, SharpCompressExtractor>();
        }
    }
}
