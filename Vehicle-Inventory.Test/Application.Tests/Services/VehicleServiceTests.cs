using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Vehicle_Inventory.Application.Common;
using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Services;
using Vehicle_Inventory.Domain.Entities;
using Xunit;

namespace Vehicle_Inventory.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepoMock;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _vehicleRepoMock = new Mock<IVehicleRepository>();
            _vehicleService = new VehicleService(_vehicleRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle("Car1", "Model1", 2021, 10000, "USD"),
                new Vehicle("Car2", "Model2", 2022, 20000, "USD")
            };

            _vehicleRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(vehicles);

            // Act
            var result = await _vehicleService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, v => v.Name == "Car1");
            Assert.Contains(result, v => v.Name == "Car2");
        }

        [Fact]
        public async Task GetByIdAsync_WhenVehicleExists_ReturnsVehicle()
        {
            // Arrange
            var vehicle = new Vehicle("Car1", "Model1", 2021, 10000, "USD");
            _vehicleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vehicle);

            // Act
            var result = await _vehicleService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Car1", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenVehicleNotFound_ThrowsValidationException()
        {
            // Arrange
            _vehicleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Vehicle?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.GetByIdAsync(1));
            Assert.Contains("VehicleNotFound", ex.Message);
        }

        [Fact]
        public async Task GetVehicleDetailsByIdAsync_WhenVehicleExists_ReturnsVehicle()
        {
            // Arrange
            var vehicle = new Vehicle("Car1", "Model1", 2021, 10000, "USD");
            _vehicleRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(vehicle);

            // Act
            var result = await _vehicleService.GetVehicleDetailsByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Car1", result.Name);
        }

        [Fact]
        public async Task GetVehicleDetailsByIdAsync_WhenVehicleNotFound_ThrowsValidationException()
        {
            _vehicleRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync((Vehicle?)null);

            var ex = await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.GetVehicleDetailsByIdAsync(1));
            Assert.Contains("VehicleNotFound", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_AddsVehicleAndReturnsId()
        {
            // Arrange
            Vehicle? addedVehicle = null;
            _vehicleRepoMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>()))
                .Callback<Vehicle>(v => addedVehicle = v)
                .Returns(Task.CompletedTask);

            // Act
            var id = await _vehicleService.CreateAsync("Car1", "Model1", 2021, 10000, "USD");

            // Assert
            Assert.NotNull(addedVehicle);
            Assert.Equal("Car1", addedVehicle!.Name);
            Assert.Equal(id, addedVehicle.Id);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageNumberOrPageSizeInvalid_ThrowsValidationException()
        {
            var ex1 = await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.GetPagedAsync(0, 10));
            Assert.Contains("PageNumberInvalid", ex1.Message);

            var ex2 = await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.GetPagedAsync(1, 0));
            Assert.Contains("PageSizeInvalid", ex2.Message);

            var ex3 = await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.GetPagedAsync(1, 101));
            Assert.Contains("PageSizeInvalid", ex3.Message);
        }

        [Fact]
        public async Task GetPagedAsync_ReturnsPagedResult()
        {
            // Arrange
            var vehicles = new List<Vehicle> { new Vehicle("Car1", "Model1", 2021, 10000, "USD") };
            _vehicleRepoMock.Setup(r => r.GetPagedAsync(1, 10)).ReturnsAsync((vehicles, 1));

            // Act
            var result = await _vehicleService.GetPagedAsync(1, 10);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetFilteredAsync_WhenMinPriceGreaterThanMaxPrice_ThrowsValidationException()
        {
            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _vehicleService.GetFilteredAsync(null, 20000, 10000, 1, 10));
            Assert.Contains("MinPriceGreaterThanMaxPrice", ex.Message);
        }

        [Fact]
        public async Task GetFilteredAsync_ReturnsPagedResult()
        {
            var vehicles = new List<Vehicle> { new Vehicle("Car1", "Model1", 2021, 10000, "USD") };
            _vehicleRepoMock.Setup(r => r.GetFilteredPagedAsync(true, null, null, 1, 10))
                .ReturnsAsync((vehicles, 1));

            var result = await _vehicleService.GetFilteredAsync(true, null, null, 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task UpdateAsync_WhenVehicleNotFound_ThrowsValidationException()
        {
            _vehicleRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync((Vehicle?)null);

            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _vehicleService.UpdateAsync(1, v => Task.CompletedTask));
            Assert.Contains("VehicleNotFound", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_CallsUpdateOnRepository()
        {
            var vehicle = new Vehicle("Car1", "Model1", 2021, 10000, "USD");
            _vehicleRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(vehicle);
            _vehicleRepoMock.Setup(r => r.UpdateAsync(vehicle)).Returns(Task.CompletedTask);

            await _vehicleService.UpdateAsync(1, v =>
            {
                v.UpdateBasicInfo("Car1Updated", "Model1", 2022, 15000, "USD", true, null, null);
                return Task.CompletedTask;
            });

            _vehicleRepoMock.Verify(r => r.UpdateAsync(vehicle), Times.Once);
            Assert.Equal("Car1Updated", vehicle.Name);
            Assert.Equal(15000, vehicle.Price);
        }

        [Fact]
        public async Task DeleteAsync_WhenVehicleNotFound_ThrowsValidationException()
        {
            _vehicleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Vehicle?)null);

            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _vehicleService.DeleteAsync(1));
            Assert.Contains("VehicleNotFound", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_CallsDeleteOnRepository()
        {
            var vehicle = new Vehicle("Car1", "Model1", 2021, 10000, "USD");
            _vehicleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vehicle);
            _vehicleRepoMock.Setup(r => r.DeleteAsync(vehicle)).Returns(Task.CompletedTask);

            await _vehicleService.DeleteAsync(1);

            _vehicleRepoMock.Verify(r => r.DeleteAsync(vehicle), Times.Once);
        }
    }
}