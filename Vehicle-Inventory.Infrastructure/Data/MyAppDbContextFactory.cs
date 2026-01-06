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
                "Server=db37382.public.databaseasp.net; Database=db37382; User Id=db37382; Password=M@l@v_1512; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;")
            .Options;

        return new MyAppDbContext(options);
    }
}