using Vehicle_Inventory.Application.DTOs.RideBooking;

namespace Vehicle_Inventory.Application.Interfaces.Services
{
    public interface ITestRideBookingService
    {
        Task<IEnumerable<SlotAvailabilityResponse>> GetAvailableSlotsAsync(
            int vehicleId,
            DateOnly date);

        Task CreateAsync(
            Guid userId,
            int vehicleId,
            DateOnly date,
            int slotIndex);

        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId);

        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();

        Task ConfirmAsync(Guid bookingId);
        Task RejectAsync(Guid bookingId);

        Task AutoRejectAsync();
        Task AutoResolveAsync();
    }
}