using AMI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMI.Persistence.InMemory.Configurations
{
    /// <summary>
    /// A configuration for the <see cref="TokenEntity"/>.
    /// </summary>
    public class TokenConfiguration : IEntityTypeConfiguration<TokenEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TokenEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.ToTable("Tokens");

            builder.Property(e => e.CreatedDate)
                .IsRequired();

            builder.Property(e => e.LastUsedDate)
                .IsRequired();

            builder.Property(e => e.TokenValue)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithMany(e => e.Tokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.LastUsedDate);
            builder.HasIndex(e => e.UserId);
        }
    }
}
