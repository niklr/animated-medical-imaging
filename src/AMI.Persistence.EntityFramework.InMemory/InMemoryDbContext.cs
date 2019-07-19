﻿using AMI.Domain.Entities;
using AMI.Persistence.EntityFramework.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AMI.Persistence.EntityFramework.InMemory
{
    /// <summary>
    /// The EntityFramework InMemory implementation of the database context.
    /// </summary>
    public class InMemoryDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
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

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}
