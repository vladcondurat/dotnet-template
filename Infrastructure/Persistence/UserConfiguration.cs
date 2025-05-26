using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
                
            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
                
            builder.Property(u => u.RefreshToken)
                .HasColumnName("refresh_token");
                
            builder.Property(u => u.RefreshTokenExpiryTime)
                .HasColumnName("refresh_token_expiry_time");

            builder.HasMany(u => u.Projects)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}