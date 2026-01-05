//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Vehicle_Inventory.Domain.Entities;

//namespace Vehicle_Inventory.Infrastructure.Data.Configurations;

//public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
//{
//    public void Configure(EntityTypeBuilder<VehicleImage> builder)
//    {
//        builder.ToTable("VehicleImages");

//        builder.HasKey(i => i.Id);

//        builder.Property(i => i.ImagePath)
//               .IsRequired()
//               .HasMaxLength(500);

//        builder.Property(i => i.DisplayOrder)
//               .IsRequired();

//        builder.Property(i => i.VehicleId)
//               .IsRequired();

//        builder.HasIndex(i => i.VehicleId);
//    }
//}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations;

public class VehicleImageConfiguration : IEntityTypeConfiguration<VehicleImage>
{
    public void Configure(EntityTypeBuilder<VehicleImage> builder)
    {
        builder.ToTable("VehicleImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.PublicId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.DisplayOrder)
            .IsRequired();

        builder.Property(i => i.VehicleId)
            .IsRequired();

        builder.HasIndex(i => i.VehicleId);

        builder.HasIndex(i => i.PublicId)
            .IsUnique();
    }
}