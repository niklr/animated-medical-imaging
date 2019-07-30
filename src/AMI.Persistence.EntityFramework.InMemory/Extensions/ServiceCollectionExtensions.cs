using AMI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Persistence.EntityFramework.InMemory.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Acts as a root for all in-memory databases such that they will be available across context instances and service providers.
        /// </summary>
        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        /// <summary>
        /// Extension method used to add the InMemory database context.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddInMemoryDbContext(this IServiceCollection services)
        {
            services.AddScoped<IAmiUnitOfWork, InMemoryUnitOfWork>();
            services.AddDbContext<InMemoryDbContext>(options =>
            {
                options.UseInMemoryDatabase("AmiInMemoryDb", InMemoryDatabaseRoot);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // remove when not using InMemory context
            });
        }
    }
}
