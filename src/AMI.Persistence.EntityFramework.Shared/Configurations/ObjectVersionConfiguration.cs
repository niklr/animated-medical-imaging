using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the ObjectVersion entity type.
    /// </summary>
    public class ObjectVersionConfiguration : IEntityTypeConfiguration<ObjectVersion>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ObjectVersion> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
}
