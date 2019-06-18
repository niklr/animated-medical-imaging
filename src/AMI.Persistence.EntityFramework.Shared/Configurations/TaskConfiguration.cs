using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
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
                .IsRequired();

            builder.Property(e => e.ModifiedDate)
                .IsRequired();

            builder.HasOne(e => e.Object)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.ObjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Result)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
