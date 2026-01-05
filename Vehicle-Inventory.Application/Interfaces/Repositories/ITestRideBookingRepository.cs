using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Interfaces.Repositories
{
    public interface ITestRideBookingRepository
    {
        Task<bool> SlotTakenAsync(
            int vehicleId,
            DateOnly date,
            int slotIndex);

        Task<List<int>> GetUnavailableSlotsAsync(
            int vehicleId,
            DateOnly date);

        Task AddAsync(TestRideBooking booking);

        Task<TestRideBooking?> GetByIdAsync(Guid id);

        Task<List<TestRideBooking>> GetByUserIdAsync(Guid userId);

        Task<List<TestRideBooking>> GetAllAsync();

        Task<List<TestRideBooking>> GetForAutoRejectAsync(DateOnly today);

        Task<List<TestRideBooking>> GetForResolveAsync(DateTime utcNow);

        Task SaveChangesAsync();
    }
}