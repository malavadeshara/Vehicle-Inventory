using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Application.DTOs.Auth;

namespace Vehicle_Inventory.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // ---------------- REGISTER ----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            Console.WriteLine("Register User DTO : ", dto.UserName);
            await _userService.RegisterAsync(
                dto.UserName,
                dto.Email,
                dto.Password);

            return StatusCode(StatusCodes.Status201Created);
        }

        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var tokens = await _authService.LoginAsync(
                dto.Email,
                dto.Password);

            return Ok(tokens);
        }

        // ---------------- REFRESH TOKEN ----------------
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto dto)
        {
            var tokens = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(tokens);
        }

        // ---------------- LOGOUT ----------------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _authService.LogoutAsync(userId);

            return NoContent();
        }
    }
}