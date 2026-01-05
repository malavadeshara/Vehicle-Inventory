using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            // Enum mapping (VERY IMPORTANT)
            builder.Property(u => u.Role)
                   .HasConversion<string>() // store enum as string
                   .IsRequired();

            // Optional refresh token fields
            builder.Property(u => u.RefreshToken)
                   .HasMaxLength(500);

            builder.Property(u => u.RefreshTokenExpiry);
        }
    }
}