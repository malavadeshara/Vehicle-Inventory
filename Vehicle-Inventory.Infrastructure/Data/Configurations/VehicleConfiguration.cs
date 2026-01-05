//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Vehicle_Inventory.Domain.Entities;

//namespace Vehicle_Inventory.Infrastructure.Data.Configurations
//{
//    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
//    {
//        public void Configure(EntityTypeBuilder<Vehicle> builder)
//        {
//            builder.ToTable("Vehicles");

//            builder.HasKey(v => v.Id);

//            builder.Property(v => v.Name)
//                   .IsRequired()
//                   .HasMaxLength(100);

//            builder.Property(v => v.Model)
//                   .IsRequired()
//                   .HasMaxLength(100);

//            builder.Property(v => v.Year)
//                   .IsRequired();

//            builder.Property(v => v.Price)
//                   .IsRequired()
//                   .HasColumnType("decimal(18,2)");

//            builder.Property(v => v.Currency)
//                   .IsRequired()
//                   .HasMaxLength(10);

//            builder.Property(v => v.InStock)
//                   .IsRequired();

//            builder.Property(v => v.ShortDescription)
//                   .HasMaxLength(500);

//            builder.Property(v => v.DetailedDescription)
//                   .HasMaxLength(2000);

//            // -------------------------
//            // Images (backing field)
//            // -------------------------
//            builder.HasMany<VehicleImage>("_images")
//                   .WithOne()
//                   .HasForeignKey(i => i.VehicleId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            builder.Navigation(v => v.Images)
//                   .UsePropertyAccessMode(PropertyAccessMode.Field);

//            // -------------------------
//            // Features (backing field)
//            // -------------------------
//            builder.HasMany<VehicleFeature>("_features")
//                   .WithOne()
//                   .HasForeignKey(f => f.VehicleId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            builder.Navigation(v => v.Features)
//                   .UsePropertyAccessMode(PropertyAccessMode.Field);

//            // -------------------------
//            // One-to-one
//            // -------------------------
//            builder.HasOne(v => v.Dimensions)
//                   .WithOne()
//                   .HasForeignKey<VehicleDimension>(d => d.VehicleId)
//                   .OnDelete(DeleteBehavior.Cascade);

//            builder.HasOne(v => v.Specifications)
//                   .WithOne()
//                   .HasForeignKey<VehicleSpecification>(s => s.VehicleId)
//                   .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(v => v.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(v => v.Year)
                   .IsRequired();

            builder.Property(v => v.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(v => v.Currency)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(v => v.InStock)
                   .IsRequired();

            builder.Property(v => v.ShortDescription)
                   .HasMaxLength(500);

            builder.Property(v => v.DetailedDescription)
                   .HasMaxLength(2000);

            // Images
            builder.HasMany(v => v.Images)
                   .WithOne()
                   .HasForeignKey(i => i.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(Vehicle.Images))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            // Features
            builder.HasMany(v => v.Features)
                   .WithOne()
                   .HasForeignKey(f => f.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(Vehicle.Features))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            // One-to-one
            builder.HasOne(v => v.Dimensions)
                   .WithOne()
                   .HasForeignKey<VehicleDimension>(d => d.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Specifications)
                   .WithOne()
                   .HasForeignKey<VehicleSpecification>(s => s.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}