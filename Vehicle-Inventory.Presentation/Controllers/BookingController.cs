using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle_Inventory.Application.DTOs.RideBooking;
using Vehicle_Inventory.Application.Interfaces.Services;

namespace Vehicle_Inventory.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ITestRideBookingService _bookingService;
        public BookingController(ITestRideBookingService testRideBookingService)
        {
            _bookingService = testRideBookingService;
        }

        //[Authorize(Roles = "Customer")]
        //[HttpGet("vehicles/{vehicleId}/slots")]
        //public async Task<IActionResult> GetSlots(int vehicleId, DateOnly date)
        //{
        //    Console.WriteLine($"GetSlots called with vehicleId: {vehicleId}, date: {date}");
        //    var result = await _bookingService
        //        .GetAvailableSlotsAsync(vehicleId, date);

        //    return Ok(result);
        //}

        [HttpGet("vehicles/{vehicleId}/slots")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetSlots(
        int vehicleId,
        [FromQuery] DateTime date
    )
        {
            // Convert DateTime → DateOnly
            var dateOnly = DateOnly.FromDateTime(date);

            var result = await _bookingService
                .GetAvailableSlotsAsync(vehicleId, dateOnly);

            return Ok(result);
        }
        //public async Task<IActionResult> GetSlots(int vehicleId, [FromQuery] DateOnly date)
        //{
        //    Console.WriteLine($"GetSlots called with vehicleId: {vehicleId}, date: {date}");
        //    return Ok(await _bookingService.GetAvailableSlotsAsync(vehicleId, date));
        //}


        //[HttpGet("my")]
        //[Authorize(Roles = "Customer")]
        //public async Task<IActionResult> MyBookings(Guid userId)
        //{
        //    return Ok(await _bookingService.GetUserBookingsAsync(userId));
        //}

        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyBookings()
        {
            // Get userId from token claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(userIdClaim.Value);

            var result = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(result);
        }


        [HttpGet("admin/get-all-bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllBookings()
        {
            return Ok(await _bookingService.GetAllBookingsAsync());
        }

        //[HttpPost]
        //[Authorize(Roles = "Customer")]
        //public async Task<IActionResult> Create(Guid userId, BookingDto request)
        //{
        //    await _bookingService.CreateAsync(userId, request.VehicleId, request.BookingDate, request.SlotIndex);
        //    return Ok(new
        //    {
        //        message= "Request sent successfully.",
        //    });
        //}

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] BookingDto request)
        {
            // Get userId from token claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(userIdClaim.Value);

            await _bookingService.CreateAsync(userId, request.VehicleId, request.BookingDate, request.SlotIndex);

            return Ok(new { message = "Request sent successfully." });
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