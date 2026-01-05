using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Infrastructure.Data.Configurations
{
    public class TestRideBookingConfiguration
        : IEntityTypeConfiguration<TestRideBooking>
    {
        public void Configure(EntityTypeBuilder<TestRideBooking> builder)
        {
            // -----------------------------
            // Table
            // -----------------------------
            builder.ToTable("TestRideBookings");

            // -----------------------------
            // Primary Key
            // -----------------------------
            builder.HasKey(x => x.Id);

            // -----------------------------
            // Properties
            // -----------------------------
            builder.Property(x => x.UserId)
                   .IsRequired();

            builder.Property(x => x.VehicleId)
                   .IsRequired();

            builder.Property(x => x.SlotIndex)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasConversion<int>() // VERY IMPORTANT
                   .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                   .IsRequired();

            builder.Property(x => x.DecisionAtUtc);

            // -----------------------------
            // DateOnly support
            // -----------------------------
            builder.Property(x => x.BookingDate)
                   .HasConversion(
                        d => d.ToDateTime(TimeOnly.MinValue),
                        d => DateOnly.FromDateTime(d))
                   .IsRequired();

            // -----------------------------
            // UNIQUE SLOT CONSTRAINT
            // -----------------------------
            builder.HasIndex(x => new
            {
                x.VehicleId,
                x.BookingDate,
                x.SlotIndex
            })
            .IsUnique();

            // -----------------------------
            // Indexes for Admin dashboards
            // -----------------------------
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.VehicleId);
        }
    }
}