using Microsoft.EntityFrameworkCore;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Infrastructure.Data;
using Vehicle_Inventory.Infrastructure.Exceptions;

namespace Vehicle_Inventory.Infrastructure.Repositories
{
    public class TestRideBookingRepository : ITestRideBookingRepository
    {
        private readonly MyAppDbContext _context;

        public TestRideBookingRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SlotTakenAsync(
            int vehicleId,
            DateOnly date,
            int slotIndex)
        {
            return await _context.TestRideBookings.AnyAsync(x =>
                x.VehicleId == vehicleId &&
                x.BookingDate == date &&
                x.SlotIndex == slotIndex &&
                x.Status != BookingStatus.Rejected &&
                x.Status != BookingStatus.AutoRejected);
        }

        public async Task<List<int>> GetUnavailableSlotsAsync(
            int vehicleId,
            DateOnly date)
        {
            return await _context.TestRideBookings
                .Where(x =>
                    x.VehicleId == vehicleId &&
                    x.BookingDate == date &&
                    x.Status != BookingStatus.Rejected &&
                    x.Status != BookingStatus.AutoRejected)
                .Select(x => x.SlotIndex)
                .Distinct()
                .ToListAsync();
        }

        public Task AddAsync(TestRideBooking booking)
            => _context.TestRideBookings.AddAsync(booking).AsTask();

        public Task<TestRideBooking?> GetByIdAsync(Guid id)
            => _context.TestRideBookings.FindAsync(id).AsTask();

        public Task<List<TestRideBooking>> GetByUserIdAsync(Guid userId)
            => _context.TestRideBookings
                .Where(x => x.UserId == userId)
                .ToListAsync();

        public Task<List<TestRideBooking>> GetAllAsync()
            => _context.TestRideBookings.ToListAsync();

        public Task<List<TestRideBooking>> GetForAutoRejectAsync(DateOnly today)
            => _context.TestRideBookings
                .Where(x =>
                    x.Status == BookingStatus.Pending &&
                    x.BookingDate.AddDays(-1) <= today)
                .ToListAsync();

        //public Task<List<TestRideBooking>> GetForResolveAsync(DateTime utcNow)
        //{
        //    return _context.TestRideBookings
        //        .Where(x =>
        //            x.Status == BookingStatus.Confirmed &&
        //            DateTime.SpecifyKind(
        //                x.BookingDate.ToDateTime(
        //                    TimeOnly.FromHours(10 + x.SlotIndex + 1)),
        //                DateTimeKind.Utc) <= utcNow)
        //        .ToListAsync();
        //}

        public Task<List<TestRideBooking>> GetForResolveAsync(DateTime utcNow)
        {
            return _context.TestRideBookings
                .Where(x =>
                    x.Status == BookingStatus.Confirmed &&
                    DateTime.SpecifyKind(
                        x.BookingDate.ToDateTime(
                            new TimeOnly(10 + x.SlotIndex + 1, 0)),
                        DateTimeKind.Utc) <= utcNow)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InfrastructureException(
                    InfrastructureErrorCode.DatabaseConcurrencyFailed, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InfrastructureException(
                    InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }
    }

}
