using AMI.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AMI.Persistence.EntityFramework.SQLite.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method used to add the SQLite database context.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddSqliteDbContext(this IServiceCollection services)
        {
            services.AddScoped<IAmiUnitOfWork, SqliteUnitOfWork>();
            services.AddDbContext<SqliteDbContext>();
        }
    }
}
