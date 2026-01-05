using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities
{
    public class TestRideBooking
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public int VehicleId { get; private set; }

        public DateOnly BookingDate { get; private set; }
        public int SlotIndex { get; private set; }

        public BookingStatus Status { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? DecisionAtUtc { get; private set; }

        private TestRideBooking() { }

        public TestRideBooking(
            Guid userId,
            int vehicleId,
            DateOnly bookingDate,
            int slotIndex)
        {
            if (bookingDate.DayOfWeek == DayOfWeek.Sunday)
                throw new DomainException(DomainErrorCode.BookingOnSundayNotAllowed);

            if (slotIndex < 0 || slotIndex > 8)
                throw new DomainException(DomainErrorCode.BookingSlotInvalid);

            Id = Guid.NewGuid();
            UserId = userId;
            VehicleId = vehicleId;
            BookingDate = bookingDate;
            SlotIndex = slotIndex;
            Status = BookingStatus.Pending;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Confirm()
        {
            Status = BookingStatus.Confirmed;
            DecisionAtUtc = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = BookingStatus.Rejected;
            DecisionAtUtc = DateTime.UtcNow;
        }

        public void AutoReject()
        {
            Status = BookingStatus.AutoRejected;
            DecisionAtUtc = DateTime.UtcNow;
        }

        public void Resolve()
        {
            if (Status != BookingStatus.Confirmed)
                throw new DomainException(DomainErrorCode.BookingAlreadyResolved);

            Status = BookingStatus.Resolved;
        }
    }
}