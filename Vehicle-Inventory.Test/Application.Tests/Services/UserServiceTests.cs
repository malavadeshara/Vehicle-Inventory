using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Services;
using Vehicle_Inventory.Domain.Entities;
using Xunit;

namespace Vehicle_Inventory.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();

            _userService = new UserService(_userRepoMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExist_AddsUser()
        {
            // Arrange
            var userName = "TestUser";
            var email = "test@example.com";
            var password = "Password123";

            _userRepoMock.Setup(r => r.ExistsAsync(userName, email)).ReturnsAsync(false);
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<User>(), password))
                .Returns("hashedPassword");

            // Act
            await _userService.RegisterAsync(userName, email, password);

            // Assert
            _userRepoMock.Verify(r => r.ExistsAsync(userName, email), Times.Once);
            _passwordHasherMock.Verify(h => h.HashPassword(It.IsAny<User>(), password), Times.Once);
            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.UserName == userName &&
                u.Email == email &&
                u.PasswordHash == "hashedPassword"
            )), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserAlreadyExists_ThrowsValidationException()
        {
            // Arrange
            var userName = "ExistingUser";
            var email = "existing@example.com";

            _userRepoMock.Setup(r => r.ExistsAsync(userName, email)).ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _userService.RegisterAsync(userName, email, "anyPassword"));

            // Assert based on exception message instead of ErrorCode
            Assert.Contains("UserAlreadyExists", ex.Message);

            // Verify that AddAsync was never called
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task getByIdAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("TestUser", "test@example.com");

            // Use reflection to set the Id since constructor generates a new GUID
            var idProp = typeof(User).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idProp!.SetValue(user, userId);

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.getByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task getByIdAsync_WhenUserDoesNotExist_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => _userService.getByIdAsync(userId));

            // Assert based on message
            Assert.Contains("UserNotFound", ex.Message);
        }
    }
}