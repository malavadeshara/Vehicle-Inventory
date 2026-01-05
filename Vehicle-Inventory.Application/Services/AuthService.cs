//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Vehicle_Inventory.Application.DTOs.Auth;
//using Vehicle_Inventory.Application.Interfaces.Repositories;
//using Vehicle_Inventory.Application.Interfaces.Services;
//using Vehicle_Inventory.Domain.Entities;

//namespace Vehicle_Inventory.Infrastructure.Services;

//public class AuthService : IAuthService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IPasswordHasher<User> _passwordHasher;
//    private readonly IConfiguration _configuration;

//    public AuthService(
//        IUserRepository userRepository,
//        IPasswordHasher<User> passwordHasher,
//        IConfiguration configuration)
//    {
//        _userRepository = userRepository;
//        _passwordHasher = passwordHasher;
//        _configuration = configuration;
//    }

//    // ---------------- LOGIN ----------------
//    public async Task<TokenResponseDto> LoginAsync(string email, string password)
//    {
//        var user = await _userRepository.GetByEmailAsync(email)
//            ?? throw new UnauthorizedAccessException("Invalid credentials");

//        var result = _passwordHasher.VerifyHashedPassword(
//            user, user.PasswordHash, password);

//        if (result == PasswordVerificationResult.Failed)
//            throw new UnauthorizedAccessException("Invalid credentials");

//        var accessToken = GenerateJwt(user);
//        var refreshToken = GenerateRefreshToken();

//        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
//        await _userRepository.UpdateAsync(user);

//        return new TokenResponseDto(accessToken, refreshToken); // TokenResponseDto do not contain constructure that contain 2 argument
//    }

//    // ---------------- REFRESH TOKEN ----------------
//    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
//    {
//        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken)
//            ?? throw new UnauthorizedAccessException("Invalid refresh token");

//        if (user.RefreshTokenExpiry < DateTime.UtcNow)
//            throw new UnauthorizedAccessException("Refresh token expired");

//        var newAccessToken = GenerateJwt(user);
//        var newRefreshToken = GenerateRefreshToken();

//        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
//        await _userRepository.UpdateAsync(user);

//        return new TokenResponseDto(newAccessToken, newRefreshToken);
//    }

//    // ---------------- LOGOUT ----------------
//    public async Task LogoutAsync(Guid userId)
//    {
//        var user = await _userRepository.GetByIdAsync(userId)
//            ?? throw new InvalidOperationException("User not found");

//        user.ClearRefreshToken();
//        await _userRepository.UpdateAsync(user);
//    }

//    // ---------------- JWT ----------------
//    private string GenerateJwt(User user)
//    {
//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//            new Claim(ClaimTypes.Email, user.Email),
//            new Claim(ClaimTypes.Role, user.Role.ToString())
//        };

//        var key = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            issuer: _configuration["Jwt:Issuer"],
//            audience: _configuration["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddMinutes(15),
//            signingCredentials: creds);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }

//    private static string GenerateRefreshToken()
//        => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
//}



using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.DTOs.Auth;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<TokenResponseDto> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
                   ?? throw new ValidationException(ValidationErrorCode.InvalidCredentials);

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            Console.WriteLine("Password verification failed for user: " + email);
            throw new ValidationException(ValidationErrorCode.InvalidCredentials);
        }
            

        var accessToken = GenerateJwt(user);
        var refreshToken = GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await _userRepository.UpdateAsync(user);

        return new TokenResponseDto(accessToken, refreshToken);
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken)
                   ?? throw new ValidationException(ValidationErrorCode.InvalidRefreshToken);

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new ValidationException(ValidationErrorCode.RefreshTokenExpired);

        var newAccessToken = GenerateJwt(user);
        var newRefreshToken = GenerateRefreshToken();

        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await _userRepository.UpdateAsync(user);

        return new TokenResponseDto(newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new ValidationException(ValidationErrorCode.UserNotFound);

        user.ClearRefreshToken();
        await _userRepository.UpdateAsync(user);
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}