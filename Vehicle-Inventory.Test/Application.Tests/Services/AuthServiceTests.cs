using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Application.Services;
using Xunit;

namespace Vehicle_Inventory.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _configurationMock = new Mock<IConfiguration>();

            // Mock JWT settings
            _configurationMock.Setup(c => c["JwtSettings:Secret"]).Returns("MySuperSecretKey12345AVeryLongSuperKey");
            _configurationMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");

            _authService = new AuthService(
                _userRepoMock.Object,
                _passwordHasherMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsTokenResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User("TestUser", email);
            user.SetRefreshToken("oldToken", DateTime.UtcNow.AddDays(1));

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, password))
                .Returns(PasswordVerificationResult.Success);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ThrowsValidationException()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongPassword";
            var user = new User("TestUser", email);

            _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, password))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authService.LoginAsync(email, password));
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentUser_ThrowsValidationException()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByEmailAsync("unknown@example.com")).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authService.LoginAsync("unknown@example.com", "any"));
        }

        [Fact]
        public async Task RefreshTokenAsync_WithValidToken_ReturnsNewTokens()
        {
            // Arrange
            var oldRefreshToken = "oldRefreshToken";
            var user = new User("TestUser", "test@example.com");
            user.SetRefreshToken(oldRefreshToken, DateTime.UtcNow.AddHours(1));

            _userRepoMock.Setup(r => r.GetByRefreshTokenAsync(oldRefreshToken)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RefreshTokenAsync(oldRefreshToken);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
            Assert.NotEqual(oldRefreshToken, result.RefreshToken);
            _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        //public async Task RefreshTokenAsync_WithExpiredToken_ThrowsValidationException()
        //{
        //    // Arrange
        //    var expiredToken = "expiredToken";
        //    var user = new User("TestUser", "test@example.com");
        //    user.SetRefreshToken(expiredToken, DateTime.UtcNow.AddHours(-1));

        //    _userRepoMock.Setup(r => r.GetByRefreshTokenAsync(expiredToken)).ReturnsAsync(user);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<ValidationException>(() => _authService.RefreshTokenAsync(expiredToken));
        //}

        [Fact]
        public async Task RefreshTokenAsync_WithExpiredToken_ThrowsValidationException()
        {
            // Arrange
            var expiredToken = "expiredToken";
            var user = new User("TestUser", "test@example.com");

            // Use reflection to bypass private set and set an expired refresh token
            var refreshTokenProp = typeof(User).GetProperty("RefreshToken",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            refreshTokenProp!.SetValue(user, expiredToken);

            var refreshExpiryProp = typeof(User).GetProperty("RefreshTokenExpiry",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            refreshExpiryProp!.SetValue(user, DateTime.UtcNow.AddHours(-1)); // expired

            _userRepoMock.Setup(r => r.GetByRefreshTokenAsync(expiredToken)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authService.RefreshTokenAsync(expiredToken));
        }

        [Fact]
        public async Task LogoutAsync_ClearsRefreshToken()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("TestUser", "test@example.com");
            user.SetRefreshToken("someToken", DateTime.UtcNow.AddDays(1));

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            // Act
            await _authService.LogoutAsync(userId);

            // Assert
            Assert.Null(user.RefreshToken);
            Assert.Null(user.RefreshTokenExpiry);
            _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task LogoutAsync_WithInvalidUser_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _authService.LogoutAsync(userId));
        }
    }
}