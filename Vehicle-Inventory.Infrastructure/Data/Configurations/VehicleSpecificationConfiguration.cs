using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations;

public class VehicleSpecificationConfiguration : IEntityTypeConfiguration<VehicleSpecification>
{
    public void Configure(EntityTypeBuilder<VehicleSpecification> builder)
    {
        builder.ToTable("VehicleSpecifications");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Engine)
               .HasMaxLength(100);

        builder.Property(s => s.Power)
               .HasMaxLength(50);

        builder.Property(s => s.Torque)
               .HasMaxLength(50);

        builder.Property(s => s.FuelType)
               .HasMaxLength(50);

        builder.Property(s => s.Transmission)
               .HasMaxLength(50);

        builder.Property(s => s.Mileage)
               .HasMaxLength(50);

        builder.Property(s => s.TopSpeed)
               .HasMaxLength(50);

        builder.Property(s => s.Acceleration)
               .HasMaxLength(50);

        builder.Property(s => s.BodyType)
               .HasMaxLength(50);

        builder.Property(s => s.Drivetrain)
               .HasMaxLength(50);

        builder.Property(s => s.Seating)
               .IsRequired();

        builder.Property(s => s.VehicleId)
               .IsRequired();

        builder.HasIndex(s => s.VehicleId)
               .IsUnique(); // One-to-one
    }
}