using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="UserEntity"/>.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Users");

            builder.Property(e => e.CreatedDate)
                .IsRequired();

            builder.Property(e => e.ModifiedDate)
                .IsRequired();

            builder.Property(e => e.Username)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.NormalizedUsername)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.NormalizedEmail)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}
