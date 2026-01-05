using Vehicle_Inventory.Application.DTOs.Auth;

namespace Vehicle_Inventory.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(string email, string password);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(Guid userId);
    }
}