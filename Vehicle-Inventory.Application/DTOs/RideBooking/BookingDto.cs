using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.DTOs.RideBooking
{
    public record BookingDto(
        Guid Id,
        int VehicleId,
        DateOnly BookingDate,
        int SlotIndex,
        BookingStatus Status
    );
}