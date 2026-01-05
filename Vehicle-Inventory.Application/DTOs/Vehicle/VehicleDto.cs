//namespace Vehicle_Inventory.Application.DTOs.Vehicle;

//public class VehicleDto
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

//    public List<string> Images { get; set; } = new();
//    public List<string> Features { get; set; } = new();

//    public VehicleSpecificationDto Specifications { get; set; } = null!;
//    public VehicleDimensionDto Dimensions { get; set; } = null!;
//}


namespace Vehicle_Inventory.Application.DTOs.Vehicle;

public class VehicleDto
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

    public List<string> Images { get; set; } = new();

    public List<string> Features { get; set; } = new();

    public VehicleSpecificationDto? Specifications { get; set; }
    public VehicleDimensionDto? Dimensions { get; set; }
}