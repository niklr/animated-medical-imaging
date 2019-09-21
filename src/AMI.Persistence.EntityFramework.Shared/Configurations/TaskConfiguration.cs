using System;
using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.EntityFramework.Shared.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="TaskEntity"/>.
    /// </summary>
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Tasks");

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.QueuedDate)
                .HasConversion(e => e, e => e.HasValue ? DateTime.SpecifyKind(e.Value, DateTimeKind.Utc) : e);

            builder.Property(e => e.StartedDate)
                .HasConversion(e => e, e => e.HasValue ? DateTime.SpecifyKind(e.Value, DateTimeKind.Utc) : e);

            builder.Property(e => e.EndedDate)
                .HasConversion(e => e, e => e.HasValue ? DateTime.SpecifyKind(e.Value, DateTimeKind.Utc) : e);

            builder.Property(e => e.Status)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(e => e.Object)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.ObjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Result)
                .WithOne(e => e.Task)
                .HasForeignKey<TaskEntity>(e => e.ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Indexes
            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => new { e.ObjectId, e.Status });
            builder.HasIndex(e => e.UserId);
        }
    }
}
