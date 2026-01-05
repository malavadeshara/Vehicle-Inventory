//using Microsoft.EntityFrameworkCore;
//using Vehicle_Inventory.Domain.Entities;
//using Vehicle_Inventory.Infrastructure.Exceptions;

//namespace Vehicle_Inventory.Infrastructure.Data;

//public class MyAppDbContext : DbContext
//{
//    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
//        : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);
//        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);
//    }

//    public DbSet<User> Users => Set<User>();
//    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
//    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
//    public DbSet<VehicleSpecification> VehicleSpecifications => Set<VehicleSpecification>();
//    public DbSet<VehicleDimension> VehicleDimensions => Set<VehicleDimension>();
//    public DbSet<VehicleFeature> VehicleFeatures => Set<VehicleFeature>();

//    public override async Task<int> SaveChangesAsync(
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            return await base.SaveChangesAsync(cancellationToken);
//        }
//        catch (DbUpdateException ex)
//        {
//            throw new InfrastructureException(
//                InfrastructureErrorCode.DatabaseUpdateFailed, ex);
//        }
//        catch (Exception ex)
//        {
//            throw new InfrastructureException(
//                InfrastructureErrorCode.DatabaseOperationFailed, ex);
//        }
//    }
//}




using Microsoft.EntityFrameworkCore;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Infrastructure.Exceptions;

public class MyAppDbContext : DbContext
{
    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
    public DbSet<VehicleSpecification> VehicleSpecifications => Set<VehicleSpecification>();
    public DbSet<VehicleDimension> VehicleDimensions => Set<VehicleDimension>();
    public DbSet<VehicleFeature> VehicleFeatures => Set<VehicleFeature>();
    public DbSet<TestRideBooking> TestRideBookings => Set<TestRideBooking>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InfrastructureException(
                InfrastructureErrorCode.DatabaseConcurrencyFailed, ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InfrastructureException(
                InfrastructureErrorCode.DatabaseUpdateFailed, ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException(
                InfrastructureErrorCode.DatabaseOperationFailed, ex);
        }
    }
}