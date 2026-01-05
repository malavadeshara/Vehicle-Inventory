using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Vehicle_Inventory.API.Controllers;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Application.Exceptions;
using Xunit;

namespace Vehicle_Inventory.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetCurrentUser_ReturnsOk_WithUserData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("TestUser", "test@example.com");

            // Set private Id via reflection
            typeof(User)
                .GetProperty("Id")!
                .SetValue(user, userId);

            _userServiceMock
                .Setup(s => s.getByIdAsync(userId))
                .ReturnsAsync(user);

            // Mock authenticated user
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    },
                    "TestAuth"
                )
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act
            var result = await _controller.GetCurrentUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = okResult.Value!;
            var responseType = response.GetType();

            Assert.Equal(userId, responseType.GetProperty("Id")!.GetValue(response));
            Assert.Equal("TestUser", responseType.GetProperty("UserName")!.GetValue(response));
            Assert.Equal("test@example.com", responseType.GetProperty("Email")!.GetValue(response));
            Assert.Equal("Customer", responseType.GetProperty("Role")!.GetValue(response));

            _userServiceMock.Verify(s => s.getByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCurrentUser_ThrowsValidationException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userServiceMock
                .Setup(s => s.getByIdAsync(userId))
                .ThrowsAsync(new ValidationException(ValidationErrorCode.UserNotFound));

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    },
                    "TestAuth"
                )
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(
                () => _controller.GetCurrentUser()
            );

            Assert.Contains("UserNotFound", ex.Message);
        }

        [Fact]
        public async Task GetCurrentUser_ThrowsFormatException_WhenUserIdClaimIsInvalid()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "invalid-guid")
                    },
                    "TestAuth"
                )
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => _controller.GetCurrentUser());
        }
    }
}