using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Inventory.Application.DTOs.RideBooking
{
    public record SlotAvailabilityResponse(
        int SlotIndex,
        bool IsAvailable
    );
}
