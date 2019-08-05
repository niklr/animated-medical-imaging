using System;
using System.IO;
using AMI.Core.Configurations;
using AMI.Core.Constants;
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
        private readonly IAppConfiguration configuration;
        private readonly IApplicationConstants constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="constants">The application constants.</param>
        public SqliteDbContext(
            DbContextOptions<SqliteDbContext> options,
            IAppConfiguration configuration,
            IApplicationConstants constants)
            : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(configuration.Options.WorkingDirectory, constants.SqliteDatabaseName);
            optionsBuilder.UseSqlite($"Data Source={dbPath};");
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}