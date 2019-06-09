using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the ObjectEntity.
    /// </summary>
    public class ObjectConfiguration : IEntityTypeConfiguration<ObjectEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ObjectEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Objects");

            builder.Property(e => e.CreatedDate)
                .IsRequired();

            builder.Property(e => e.ModifiedDate)
                .IsRequired();
        }
    }
}
