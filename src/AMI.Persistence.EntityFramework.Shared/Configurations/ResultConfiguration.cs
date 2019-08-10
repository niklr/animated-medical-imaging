using System;
using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="ResultEntity"/>.
    /// </summary>
    public class ResultConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Results");

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));

            builder.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasConversion(e => e, e => DateTime.SpecifyKind(e, DateTimeKind.Utc));
        }
    }
}
