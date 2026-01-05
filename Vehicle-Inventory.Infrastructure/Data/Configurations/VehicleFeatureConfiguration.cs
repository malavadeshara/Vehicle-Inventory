using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations;

public class VehicleFeatureConfiguration : IEntityTypeConfiguration<VehicleFeature>
{
    public void Configure(EntityTypeBuilder<VehicleFeature> builder)
    {
        builder.ToTable("VehicleFeatures");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FeatureName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(f => f.VehicleId)
               .IsRequired();

        builder.HasIndex(f => new { f.VehicleId, f.FeatureName })
               .IsUnique(); // Prevent duplicate features per vehicle
    }
}