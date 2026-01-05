using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Inventory.Domain.Entities
{
    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2,
        Rejected = 3,
        AutoRejected = 4,
        Resolved = 5
    }
}