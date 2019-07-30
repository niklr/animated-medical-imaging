using AMI.Domain.Entities;
using AMI.Persistence.EntityFramework.Shared;
using AMI.Persistence.EntityFramework.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.SQLite
{
    /// <summary>
    /// The EntityFramework SQLite implementation of the database context.
    /// </summary>
    public class SqliteDbContext : SharedDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=AmiSqliteDatabase.db;");
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}