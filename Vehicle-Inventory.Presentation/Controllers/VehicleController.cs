using Microsoft.AspNetCore.Mvc;
using Vehicle_Inventory.Application.Common;
using Vehicle_Inventory.Application.DTOs.Vehicle;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Vehicle_Inventory.API.Controllers;

[ApiController]
[Route("api/vehicles")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IImageStorageService _imageStorageService;

    public VehicleController(
        IVehicleService vehicleService,
        IImageStorageService imageStorageService)
    {
        _vehicleService = vehicleService;
        _imageStorageService = imageStorageService;
    }

    // ---------------- CREATE ----------------
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] CreateVehicleDto dto)
    {
        var vehicleId = await _vehicleService.CreateAsync(
            dto.Name,
            dto.Model,
            dto.Year,
            dto.Price,
            dto.Currency);

        await _vehicleService.UpdateAsync(vehicleId, async vehicle =>
        {
            vehicle.UpdateBasicInfo(
                dto.Name,
                dto.Model,
                dto.Year,
                dto.Price,
                dto.Currency,
                dto.InStock,
                dto.ShortDescription,
                dto.DetailedDescription);

            if (dto.Images != null)
            {
                int order = 1;
                foreach (var image in dto.Images)
                {
                    var publicId = Guid.NewGuid().ToString();
                    var upload = await _imageStorageService.UploadAsync(
                        image.OpenReadStream(),
                        $"vehicles/{vehicleId}",
                        publicId);

                    vehicle.AddImage(new VehicleImage(
                        upload.PublicId,
                        upload.SecureUrl,
                        order++));
                }
            }

            foreach (var feature in dto.Features)
                vehicle.AddFeature(new VehicleFeature(feature));

            vehicle.SetDimensions(new VehicleDimension(dto.Dimensions.Length, dto.Dimensions.Width, dto.Dimensions.Height, dto.Dimensions.Wheelbase, dto.Dimensions.BootSpace));

            vehicle.SetSpecifications(new VehicleSpecification(
                dto.Specifications.Seating,
                dto.Specifications.Engine,
                dto.Specifications.Power,
                dto.Specifications.Torque,
                dto.Specifications.FuelType,
                dto.Specifications.Transmission,
                dto.Specifications.Mileage,
                dto.Specifications.TopSpeed,
                dto.Specifications.Acceleration,
                dto.Specifications.BodyType,
                dto.Specifications.Drivetrain
            ));
        });

        return CreatedAtAction(nameof(GetById), new { id = vehicleId }, null);
    }


    // ---------------- UPDATE ----------------
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateVehicleDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        await _vehicleService.UpdateAsync(id, async vehicle =>
        {
            vehicle.UpdateBasicInfo(
                dto.Name,
                dto.Model,
                dto.Year,
                dto.Price,
                dto.Currency,
                dto.InStock,
                dto.ShortDescription,
                dto.DetailedDescription);

            //if (dto.RemovedImages != null)
            //{
            //    foreach (var publicId in dto.RemovedImages) // only the name is public id but right noe it is url later it will be changed with publidId
            //    {
            //        vehicle.RemoveImage(publicId);
            //        //await _imageStorageService.DeleteAsync(publicId);
            //    }
            //}

            if (dto.RemovedImages != null)
            {
                foreach (var imageUrl in dto.RemovedImages) // only the name is public id but right noe it is url later it will be changed with publidId
                {
                    vehicle.RemoveImage(imageUrl);
                    //await _imageStorageService.DeleteAsync(publicId);
                }
            }

            if (dto.NewImages != null)
            {
                int order = vehicle.Images.Count + 1;
                foreach (var image in dto.NewImages)
                {
                    var publicId = Guid.NewGuid().ToString();
                    var upload = await _imageStorageService.UploadAsync(
                        image.OpenReadStream(),
                        $"vehicles/{id}",
                        publicId);

                    vehicle.AddImage(new VehicleImage(
                        upload.PublicId,
                        upload.SecureUrl,
                        order++));
                }
            }

            vehicle.ClearFeatures();
            foreach (var feature in dto.Features)
                vehicle.AddFeature(new VehicleFeature(feature));




            //// Remove deleted features
            //var dtoFeatureIds = dto.Features
            //    .Where(f => f.Id.HasValue)
            //    .Select(f => f.Id!.Value)
            //    .ToHashSet();

            //var toRemove = vehicle.Features
            //    .Where(f => f.Id != 0 && !dtoFeatureIds.Contains(f.Id))
            //    .ToList();

            //foreach (var feature in toRemove)
            //    vehicle.RemoveFeature(feature.Id);

            //// Add or update features
            //foreach (var dtoFeature in dto.Features)
            //{
            //    if (dtoFeature.Id.HasValue)
            //    {
            //        var feature = vehicle.Features
            //            .First(f => f.Id == dtoFeature.Id.Value);

            //        feature.UpdateName(dtoFeature.Name);
            //    }
            //    else
            //    {
            //        vehicle.AddFeature(new VehicleFeature(dtoFeature.Name));
            //    }
            //}

            vehicle.SetDimensions(new VehicleDimension(dto.Dimensions.Length, dto.Dimensions.Width, dto.Dimensions.Height, dto.Dimensions.Wheelbase, dto.Dimensions.BootSpace));

            vehicle.SetSpecifications(new VehicleSpecification(
                dto.Specifications.Seating,
                dto.Specifications.Engine,
                dto.Specifications.Power,
                dto.Specifications.Torque,
                dto.Specifications.FuelType,
                dto.Specifications.Transmission,
                dto.Specifications.Mileage,
                dto.Specifications.TopSpeed,
                dto.Specifications.Acceleration,
                dto.Specifications.BodyType,
                dto.Specifications.Drivetrain
            ));

        });

        return NoContent();
    }


    // ---------------- GET BY ID ----------------
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<VehicleDto>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetVehicleDetailsByIdAsync(id);

        return Ok(MapToDto(vehicle));
    }

    // ---------------- GET PAGED ----------------
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged(
    int pageNumber = 1,
    int pageSize = 10)
    {
        var result = await _vehicleService.GetPagedAsync(pageNumber, pageSize);

        var dtoItems = result.Items
            .Select(MapToDto)
            .ToList();

        return Ok(PagedResult<VehicleDto>.Create(
            dtoItems,
            result.PageNumber,
            result.PageSize,
            result.TotalCount));
    }

    // ---------------- GET FILTERED ----------------
    [HttpGet("filter")]
    [Authorize]
    public async Task<IActionResult> GetFiltered(
    bool? inStock,
    decimal? minPrice,
    decimal? maxPrice,
    int pageNumber = 1,
    int pageSize = 10)
    {
        var result = await _vehicleService.GetFilteredAsync(
            inStock,
            minPrice,
            maxPrice,
            pageNumber,
            pageSize);

        var dtoItems = result.Items
            .Select(MapToDto)
            .ToList();

        return Ok(PagedResult<VehicleDto>.Create(
            dtoItems,
            result.PageNumber,
            result.PageSize,
            result.TotalCount));
    }

    // ---------------- DELETE ----------------
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _vehicleService.DeleteAsync(id);
        return NoContent();
    }

    // ---------------- MAPPING ----------------
    private static VehicleDto MapToDto(Vehicle vehicle)
    {
        return new VehicleDto
        {
            Id = vehicle.Id,
            Name = vehicle.Name,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Price = vehicle.Price,
            Currency = vehicle.Currency,
            InStock = vehicle.InStock,
            ShortDescription = vehicle.ShortDescription ?? string.Empty,
            DetailedDescription = vehicle.DetailedDescription ?? string.Empty,

            // return Cloudinary SecureUrl
            Images = vehicle.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => i.ImageUrl)
                .ToList(),

            Features = vehicle.Features
                .Select(f => f.FeatureName)
                .ToList(),

            Dimensions = vehicle.Dimensions == null ? null : new VehicleDimensionDto
            {
                Length = vehicle.Dimensions.Length!,
                Width = vehicle.Dimensions.Width!,
                Height = vehicle.Dimensions.Height!,
                Wheelbase = vehicle.Dimensions.Wheelbase!,
                BootSpace = vehicle.Dimensions.BootSpace!
            },

            Specifications = vehicle.Specifications == null ? null : new VehicleSpecificationDto
            {
                Seating = vehicle.Specifications.Seating,
                Engine = vehicle.Specifications.Engine!,
                Power = vehicle.Specifications.Power!,
                Torque = vehicle.Specifications.Torque!,
                FuelType = vehicle.Specifications.FuelType!,
                Transmission = vehicle.Specifications.Transmission!,
                Mileage = vehicle.Specifications.Mileage!,
                TopSpeed = vehicle.Specifications.TopSpeed!,
                Acceleration = vehicle.Specifications.Acceleration!,
                BodyType = vehicle.Specifications.BodyType!,
                Drivetrain = vehicle.Specifications.Drivetrain!
            }
        };
    }
}