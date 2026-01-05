using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vehicle_Inventory.Application.Interfaces.Services;

namespace Vehicle_Inventory.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ---------------- GET CURRENT USER ----------------
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _userService.getByIdAsync(userId);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Role = user.Role.ToString()
            });
        }
    }
}