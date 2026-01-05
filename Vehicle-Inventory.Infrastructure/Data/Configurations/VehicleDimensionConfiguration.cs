using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations;

public class VehicleDimensionConfiguration : IEntityTypeConfiguration<VehicleDimension>
{
    public void Configure(EntityTypeBuilder<VehicleDimension> builder)
    {
        builder.ToTable("VehicleDimensions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Length)
               .HasMaxLength(50);

        builder.Property(d => d.Width)
               .HasMaxLength(50);

        builder.Property(d => d.Height)
               .HasMaxLength(50);

        builder.Property(d => d.Wheelbase)
               .HasMaxLength(50);

        builder.Property(d => d.BootSpace)
               .HasMaxLength(50);

        builder.Property(d => d.VehicleId)
               .IsRequired();

        builder.HasIndex(d => d.VehicleId)
               .IsUnique(); // One-to-one
    }
}