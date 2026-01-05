namespace Vehicle_Inventory.Application.DTOs.Vehicle;

public class VehicleSpecificationDto
{
    public string Engine { get; set; } = null!;
    public string Power { get; set; } = null!;
    public string Torque { get; set; } = null!;
    public string FuelType { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string Mileage { get; set; } = null!;
    public string TopSpeed { get; set; } = null!;
    public string Acceleration { get; set; } = null!;
    public int Seating { get; set; }
    public string BodyType { get; set; } = null!;
    public string Drivetrain { get; set; } = null!;
}