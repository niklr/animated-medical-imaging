using System;
using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="AuditEventEntity"/>.
    /// </summary>
    public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEventEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AuditEventEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("AuditEvents");

            builder.Property(e => e.Timestamp)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.EventType)
                .IsRequired();

            builder.Property(e => e.SubEventType)
                .IsRequired();

            builder.Property(e => e.EventSerialized)
                .IsRequired();

            // Indexes
            builder.HasIndex(e => e.Timestamp);
        }
    }
}
