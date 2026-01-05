namespace Vehicle_Inventory.Domain.Entities;

public class VehicleDimension
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }

    public string? Length { get; private set; }
    public string? Width { get; private set; }
    public string? Height { get; private set; }
    public string? Wheelbase { get; private set; }
    public string? BootSpace { get; private set; }

    protected VehicleDimension() { }

    public VehicleDimension(
        string? length,
        string? width,
        string? height,
        string? wheelbase,
        string? bootSpace)
    {
        Length = length;
        Width = width;
        Height = height;
        Wheelbase = wheelbase;
        BootSpace = bootSpace;
    }
}