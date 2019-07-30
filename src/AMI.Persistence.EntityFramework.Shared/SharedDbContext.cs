using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.Shared
{
    /// <summary>
    /// The shraed database context.
    /// </summary>
    public abstract class SharedDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharedDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public SharedDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the object entities.
        /// </summary>
        public DbSet<ObjectEntity> Objects { get; set; }

        /// <summary>
        /// Gets or sets the result entities.
        /// </summary>
        public DbSet<ResultEntity> Results { get; set; }

        /// <summary>
        /// Gets or sets the task entities.
        /// </summary>
        public DbSet<TaskEntity> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the token entities.
        /// </summary>
        public DbSet<TokenEntity> Tokens { get; set; }

        /// <summary>
        /// Gets or sets the user entities.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }
    }
}
