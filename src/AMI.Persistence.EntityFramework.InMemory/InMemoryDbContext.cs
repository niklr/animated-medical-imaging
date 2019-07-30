using AMI.Persistence.EntityFramework.Shared;
using AMI.Persistence.EntityFramework.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.InMemory
{
    /// <summary>
    /// The EntityFramework InMemory implementation of the database context.
    /// </summary>
    public class InMemoryDbContext : SharedDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}
