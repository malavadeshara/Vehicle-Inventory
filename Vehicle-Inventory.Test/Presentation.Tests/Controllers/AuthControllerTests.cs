using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Vehicle_Inventory.API.Controllers;
using Vehicle_Inventory.Application.DTOs.Auth;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Application.Exceptions;
using Xunit;

namespace Vehicle_Inventory.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userServiceMock = new Mock<IUserService>();

            _controller = new AuthController(_authServiceMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task Register_Returns201Created_WhenSuccessful()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "Password123"
            };

            _userServiceMock.Setup(s => s.RegisterAsync(dto.UserName, dto.Email, dto.Password))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusResult.StatusCode);
            _userServiceMock.Verify(s => s.RegisterAsync(dto.UserName, dto.Email, dto.Password), Times.Once);
        }

        [Fact]
        public async Task Register_ThrowsValidationException_WhenUserExists()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "ExistingUser",
                Email = "existing@example.com",
                Password = "Password123"
            };

            _userServiceMock
                .Setup(s => s.RegisterAsync(dto.UserName, dto.Email, dto.Password))
                .ThrowsAsync(new ValidationException(ValidationErrorCode.UserAlreadyExists));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => _controller.Register(dto));
            Assert.Contains("UserAlreadyExists", ex.Message);
        }

        [Fact]
        public async Task Login_ReturnsOkWithTokens()
        {
            var dto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            var tokenResponse = new TokenResponseDto("access-token", "refresh-token");
            _authServiceMock.Setup(s => s.LoginAsync(dto.Email, dto.Password))
                .ReturnsAsync(tokenResponse);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTokens = Assert.IsType<TokenResponseDto>(okResult.Value);
            Assert.Equal("access-token", returnedTokens.AccessToken);
            Assert.Equal("refresh-token", returnedTokens.RefreshToken);
            _authServiceMock.Verify(s => s.LoginAsync(dto.Email, dto.Password), Times.Once);
        }

        [Fact]
        public async Task Refresh_ReturnsOkWithNewTokens()
        {
            var dto = new RefreshTokenRequestDto
            {
                UserId = Guid.NewGuid(),
                RefreshToken = "old-refresh-token"
            };

            var newTokens = new TokenResponseDto("new-access", "new-refresh");

            _authServiceMock.Setup(s => s.RefreshTokenAsync(dto.RefreshToken))
                .ReturnsAsync(newTokens);

            var result = await _controller.Refresh(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTokens = Assert.IsType<TokenResponseDto>(okResult.Value);
            Assert.Equal("new-access", returnedTokens.AccessToken);
            Assert.Equal("new-refresh", returnedTokens.RefreshToken);
            _authServiceMock.Verify(s => s.RefreshTokenAsync(dto.RefreshToken), Times.Once);
        }

        [Fact]
        public async Task Logout_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Simulate authenticated user with claim
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            _authServiceMock.Setup(s => s.LogoutAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout();

            // Assert
            Assert.IsType<NoContentResult>(result);
            _authServiceMock.Verify(s => s.LogoutAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Logout_ThrowsException_WhenUserIdInvalid()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "invalid-guid")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => _controller.Logout());
        }
    }
}