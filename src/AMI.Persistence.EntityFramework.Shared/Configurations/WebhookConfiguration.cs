using System;
using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.EntityFramework.Shared.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="ObjectEntity"/>.
    /// </summary>
    public class WebhookConfiguration : IEntityTypeConfiguration<WebhookEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<WebhookEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Webhooks");

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.Url)
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(e => e.ApiVersion)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.Secret)
                .HasMaxLength(4096)
                .IsRequired();

            builder.Property(e => e.EnabledEvents)
                .HasMaxLength(4096)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            // Indexes
            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.UserId);
        }
    }
}
