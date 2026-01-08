using Vehicle_Inventory.Application.DTOs.RideBooking;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Application.Services
{
    public class TestRideBookingService : ITestRideBookingService
    {
        private readonly ITestRideBookingRepository _repo;

        public TestRideBookingService(ITestRideBookingRepository repo)
        {
            _repo = repo;
        }

        //public async Task<IEnumerable<SlotAvailabilityResponse>> GetAvailableSlotsAsync(int vehicleId, DateOnly date)
        //{
        //    // No slots on Sunday
        //    if (date.DayOfWeek == DayOfWeek.Sunday)
        //        return Enumerable.Empty<SlotAvailabilityResponse>();

        //    // Convert DateOnly to DateTime at midnight for comparison
        //    var inputDateTime = date.ToDateTime(TimeOnly.MinValue);
        //    var todayDateTime = DateTime.Now.Date;

        //    // Past date → no slots
        //    if (inputDateTime < todayDateTime)
        //        return Enumerable.Empty<SlotAvailabilityResponse>();

        //    // Fetch unavailable slots
        //    var unavailable = await _repo.GetUnavailableSlotsAsync(vehicleId, date);

        //    // Slot configuration
        //    int totalSlots = 9;
        //    TimeOnly slotStartTime = new TimeOnly(9, 0); // 9 AM
        //    int slotDurationMinutes = 60;

        //    int minAllowedSlotIndex = 0;

        //    // If today → only allow slots 1 hour after now
        //    if (inputDateTime == todayDateTime)
        //    {
        //        var now = DateTime.Now;
        //        var allowedTime = now.AddHours(1); // 1 hour buffer

        //        var minutesFromStart =
        //            (allowedTime.TimeOfDay - slotStartTime.ToTimeSpan()).TotalMinutes;

        //        minAllowedSlotIndex = (int)Math.Ceiling(minutesFromStart / slotDurationMinutes);

        //        // If all slots are already past
        //        if (minAllowedSlotIndex >= totalSlots)
        //            return Enumerable.Empty<SlotAvailabilityResponse>();
        //    }

        //    // Generate slots
        //    return Enumerable.Range(0, totalSlots)
        //        .Where(i => i >= minAllowedSlotIndex)
        //        .Select(i => new SlotAvailabilityResponse(
        //            i,
        //            !unavailable.Contains(i)
        //        ));
        //}

        public async Task<IEnumerable<SlotAvailabilityResponse>> GetAvailableSlotsAsync(int vehicleId, DateOnly date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                return Enumerable.Empty<SlotAvailabilityResponse>();

            var istZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            var nowIst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

            var todayIst = DateOnly.FromDateTime(nowIst);

            if (date < todayIst)
                return Enumerable.Empty<SlotAvailabilityResponse>();

            var unavailable = await _repo.GetUnavailableSlotsAsync(vehicleId, date);

            const int totalSlots = 9;
            var slotStartTime = new TimeOnly(9, 0);
            const int slotDurationMinutes = 60;

            var minAllowedSlotIndex = 0;

            if (date == todayIst)
            {
                var allowedTimeIst = nowIst.AddHours(1);

                var minutesFromStart =
                    (allowedTimeIst.TimeOfDay - slotStartTime.ToTimeSpan()).TotalMinutes;

                minAllowedSlotIndex =
                    (int)Math.Ceiling(minutesFromStart / slotDurationMinutes);

                if (minAllowedSlotIndex >= totalSlots)
                    return Enumerable.Empty<SlotAvailabilityResponse>();
            }

            return Enumerable.Range(0, totalSlots)
                .Where(i => i >= minAllowedSlotIndex)
                .Select(i => new SlotAvailabilityResponse(
                    i,
                    !unavailable.Contains(i)
                ));
        }

        public async Task CreateAsync(
            Guid userId,
            int vehicleId,
            DateOnly date,
            int slotIndex)
        {
            if (await _repo.SlotTakenAsync(vehicleId, date, slotIndex))
                throw new DomainException(DomainErrorCode.BookingSlotAlreadyTaken);

            var booking = new TestRideBooking(
                userId, vehicleId, date, slotIndex);

            await _repo.AddAsync(booking);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingDto>>
            GetUserBookingsAsync(Guid userId)
        {
            var bookings = await _repo.GetByUserIdAsync(userId);

            return bookings.Select(x => new BookingDto(
                x.Id, x.VehicleId, x.BookingDate, x.SlotIndex, x.Status));
        }

        public async Task<IEnumerable<BookingDto>>
            GetAllBookingsAsync()
        {
            var bookings = await _repo.GetAllAsync();

            return bookings.Select(x => new BookingDto(
                x.Id, x.VehicleId, x.BookingDate, x.SlotIndex, x.Status));
        }

        public async Task ConfirmAsync(Guid bookingId)
        {
            var booking = await _repo.GetByIdAsync(bookingId)
                ?? throw new DomainException(DomainErrorCode.BookingNotFound);

            booking.Confirm();
            await _repo.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid bookingId)
        {
            var booking = await _repo.GetByIdAsync(bookingId)
                ?? throw new DomainException(DomainErrorCode.BookingNotFound);

            booking.Reject();
            await _repo.SaveChangesAsync();
        }

        public async Task AutoRejectAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var bookings = await _repo.GetForAutoRejectAsync(today);

            bookings.ForEach(b => b.AutoReject());
            await _repo.SaveChangesAsync();
        }

        public async Task AutoResolveAsync()
        {
            var bookings = await _repo.GetForResolveAsync(DateTime.UtcNow);

            bookings.ForEach(b => b.Resolve());
            await _repo.SaveChangesAsync();
        }
    }
}