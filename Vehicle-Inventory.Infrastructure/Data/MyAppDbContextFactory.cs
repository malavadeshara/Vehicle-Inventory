using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Vehicle_Inventory.Infrastructure.Data;

public class MyAppDbContextFactory
    : IDesignTimeDbContextFactory<MyAppDbContext>
{
    public MyAppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<MyAppDbContext>()
            .UseSqlServer(
                "Server=LAPTOP-FFN4O190;Database=Vehicle_Inventory_Database;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True")
            .Options;

        return new MyAppDbContext(options);
    }
}