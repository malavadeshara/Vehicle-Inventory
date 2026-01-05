//namespace Vehicle_Inventory.Domain.Entities;

//public class VehicleSpecification
//{
//    public int Id { get; private set; }
//    public int VehicleId { get; private set; }

//    public string? Engine { get; private set; }
//    public string? Power { get; private set; }
//    public string? Torque { get; private set; }
//    public string? FuelType { get; private set; }
//    public string? Transmission { get; private set; }
//    public string? Mileage { get; private set; }
//    public string? TopSpeed { get; private set; }
//    public string? Acceleration { get; private set; }
//    public int Seating { get; private set; }
//    public string? BodyType { get; private set; }
//    public string? Drivetrain { get; private set; }

//    protected VehicleSpecification() { }

//    public VehicleSpecification(int seating, string? engine = null)
//    {
//        Seating = seating;
//        Engine = engine;
//    }
//}


using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities;

public class VehicleSpecification
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }

    public string? Engine { get; private set; }
    public string? Power { get; private set; }
    public string? Torque { get; private set; }
    public string? FuelType { get; private set; }
    public string? Transmission { get; private set; }
    public string? Mileage { get; private set; }
    public string? TopSpeed { get; private set; }
    public string? Acceleration { get; private set; }
    public int Seating { get; private set; }
    public string? BodyType { get; private set; }
    public string? Drivetrain { get; private set; }

    protected VehicleSpecification() { }

    public VehicleSpecification(
        int seating,
        string? engine = null,
        string? power = null, 
        string? torque = null,
        string? fuleType = null,
        string? transmission = null,
        string? mileage = null,
        string topSpeed = null!,
        string acceleration = null!,
        string bodyType = null!,
        string drivetrain = null!
        )
    {
        if (seating <= 0)
            throw new DomainException(DomainErrorCode.VehicleSpecificationSeatingInvalid);

        Seating = seating;
        Engine = engine;
        Power = power;
        Torque = torque;
        FuelType = fuleType;
        Transmission = transmission;
        Mileage = mileage;
        TopSpeed = topSpeed;
        Acceleration = acceleration;
        BodyType = bodyType;
        Drivetrain = drivetrain;
    }
}