using System;
using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.EntityFramework.Shared.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="EventEntity"/>.
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Events");

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.ApiVersion)
                .IsRequired();

            builder.Property(e => e.EventType)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.EventSerialized)
                .IsRequired();

            // Indexes
            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.EventType);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => new { e.CreatedDate, e.UserId });
            builder.HasIndex(e => new { e.CreatedDate, e.EventType, e.UserId });
        }
    }
}
