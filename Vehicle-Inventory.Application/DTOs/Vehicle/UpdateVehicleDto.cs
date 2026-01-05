//using Microsoft.AspNetCore.Http;

//namespace Vehicle_Inventory.Application.DTOs.Vehicle;

//public class UpdateVehicleDto
//{
//    public int Id { get; set; }

//    public string Name { get; set; } = null!;
//    public string Model { get; set; } = null!;
//    public int Year { get; set; }

//    public decimal Price { get; set; }
//    public string Currency { get; set; } = null!;

//    public bool InStock { get; set; }
//    public string AgeInShowroom { get; set; } = null!;

//    public string ShortDescription { get; set; } = null!;
//    public string DetailedDescription { get; set; } = null!;

//    // Existing image URLs (for display)
//    public List<string> ExistingImages { get; set; } = new();

//    // New uploads
//    public List<IFormFile>? NewImages { get; set; }

//    // PublicIds to delete
//    public List<string>? RemovedImages { get; set; }

//    public List<string> Features { get; set; } = new();

//    public VehicleSpecificationDto Specifications { get; set; } = null!;
//    public VehicleDimensionDto Dimensions { get; set; } = null!;
//}



using Microsoft.AspNetCore.Http;

namespace Vehicle_Inventory.Application.DTOs.Vehicle;

public class UpdateVehicleDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }

    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;

    public bool InStock { get; set; }

    public string ShortDescription { get; set; } = null!;
    public string DetailedDescription { get; set; } = null!;

    public List<IFormFile>? NewImages { get; set; }

    public List<string>? RemovedImages { get; set; }

    public List<string> Features { get; set; } = new();

    //public List<UpdateVehicleFeatureDto> Features { get; set; } = new();

    public VehicleSpecificationDto Specifications { get; set; } = null!;
    public VehicleDimensionDto Dimensions { get; set; } = null!;
}
