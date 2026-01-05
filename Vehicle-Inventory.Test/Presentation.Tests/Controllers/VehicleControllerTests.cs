using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Vehicle_Inventory.API.Controllers;
using Vehicle_Inventory.Application.Common;
using Vehicle_Inventory.Application.DTOs.Vehicle;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Xunit;

namespace Vehicle_Inventory.Tests.Controllers
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IImageStorageService> _imageStorageMock;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _vehicleServiceMock = new Mock<IVehicleService>();
            _imageStorageMock = new Mock<IImageStorageService>();

            _controller = new VehicleController(
                _vehicleServiceMock.Object,
                _imageStorageMock.Object
            );
        }

        // ---------------- CREATE ----------------

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var dto = new CreateVehicleDto
            {
                Name = "Car",
                Model = "Model X",
                Year = 2023,
                Price = 100000,
                Currency = "USD",
                InStock = true,
                Features = new List<string>(),
                Images = null,
                Dimensions = new VehicleDimensionDto
                {
                    Length = "123 mm",
                    Width = "420 mm",
                    Height = "555 mm",
                    Wheelbase = "510 mm",
                    BootSpace = "150 liter"
                },
                Specifications = new VehicleSpecificationDto
                {
                    Seating = 5,
                    Engine = "V8",
                    Power = "500hp",
                    Torque = "600Nm",
                    FuelType = "Petrol",
                    Transmission = "Auto",
                    Mileage = "10kmpl",
                    TopSpeed = "250",
                    Acceleration = "4s",
                    BodyType = "SUV",
                    Drivetrain = "AWD"
                }
            };

            _vehicleServiceMock
                .Setup(s => s.CreateAsync(dto.Name, dto.Model, dto.Year, dto.Price, dto.Currency))
                .ReturnsAsync(1);

            _vehicleServiceMock
                .Setup(s => s.UpdateAsync(1, It.IsAny<Func<Vehicle, Task>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(VehicleController.GetById), createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues!["id"]);
        }

        // ---------------- UPDATE ----------------

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            var dto = new UpdateVehicleDto { Id = 2 };

            var result = await _controller.Update(1, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            var dto = new UpdateVehicleDto
            {
                Id = 1,
                Name = "Updated",
                Model = "Updated",
                Year = 2023,
                Price = 120000,
                Currency = "USD",
                InStock = true,
                Features = new List<string>(),
                Dimensions = new VehicleDimensionDto(),
                Specifications = new VehicleSpecificationDto()
            };

            _vehicleServiceMock
                .Setup(s => s.UpdateAsync(1, It.IsAny<Func<Vehicle, Task>>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        // ---------------- GET BY ID ----------------

        [Fact]
        public async Task GetById_ReturnsOk_WithVehicleDto()
        {
            var vehicle = new Vehicle("Car", "Model", 2023, 100000, "USD");

            typeof(Vehicle).GetProperty("Id")!.SetValue(vehicle, 1);

            _vehicleServiceMock
                .Setup(s => s.GetVehicleDetailsByIdAsync(1))
                .ReturnsAsync(vehicle);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<VehicleDto>(okResult.Value);

            Assert.Equal(1, dto.Id);
            Assert.Equal("Car", dto.Name);
        }

        // ---------------- GET PAGED ----------------

        [Fact]
        public async Task GetPaged_ReturnsPagedResult()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle("Car", "Model", 2023, 100000, "USD")
            };

            _vehicleServiceMock
                .Setup(s => s.GetPagedAsync(1, 10))
                .ReturnsAsync(PagedResult<Vehicle>.Create(vehicles, 1, 10, 1));

            var result = await _controller.GetPaged(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var paged = Assert.IsType<PagedResult<VehicleDto>>(okResult.Value);

            Assert.Single(paged.Items);
            Assert.Equal(1, paged.TotalCount);
        }

        // ---------------- GET FILTERED ----------------

        [Fact]
        public async Task GetFiltered_ReturnsPagedResult()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle("Car", "Model", 2023, 100000, "USD")
            };

            _vehicleServiceMock
                .Setup(s => s.GetFilteredAsync(true, null, null, 1, 10))
                .ReturnsAsync(PagedResult<Vehicle>.Create(vehicles, 1, 10, 1));

            var result = await _controller.GetFiltered(true, null, null, 1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var paged = Assert.IsType<PagedResult<VehicleDto>>(okResult.Value);

            Assert.Single(paged.Items);
        }

        // ---------------- DELETE ----------------

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            _vehicleServiceMock
                .Setup(s => s.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            _vehicleServiceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}