using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.EntityFramework.Shared.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="RoleEntity"/>.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Roles");

            builder.Property(e => e.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.NormalizedName)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
