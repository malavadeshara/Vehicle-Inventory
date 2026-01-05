using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Inventory.Application.DTOs.RideBooking;
using Vehicle_Inventory.Application.Interfaces.Services;

namespace Vehicle_Inventory.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ITestRideBookingService _bookingService;
        public BookingController(ITestRideBookingService testRideBookingService)
        {
            _bookingService = testRideBookingService;
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("vehicles/{vehicleId}/slots")]
        public async Task<IActionResult> GetSlots(int vehicleId, DateOnly date)
        {
            var result = await _bookingService
                .GetAvailableSlotsAsync(vehicleId, date);

            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyBookings(Guid userId)
        {
            return Ok(await _bookingService.GetUserBookingsAsync(userId));
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllBookings()
        {
            return Ok(await _bookingService.GetAllBookingsAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(Guid userId, BookingDto request)
        {
            await _bookingService.CreateAsync(userId, request.VehicleId, request.BookingDate, request.SlotIndex);
            return Ok(new
            {
                message= "Request sent successfully.",
            });
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            await _bookingService.ConfirmAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(Guid id)
        {
            await _bookingService.RejectAsync(id);
            return NoContent();
        }
    }
}
