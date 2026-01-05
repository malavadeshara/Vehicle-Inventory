//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace Vehicle_Inventory.Infrastructure.Data
//{
//    public class MyAppDbContextFactory
//        : IDesignTimeDbContextFactory<MyAppDbContext>
//    {
//        public MyAppDbContext CreateDbContext(string[] args)
//        {
//            // This is the API project directory at design-time
//            var basePath = Directory.GetCurrentDirectory();

//            var configuration = new ConfigurationBuilder()
//                .SetBasePath(basePath)
//                .AddJsonFile("appsettings.json", optional: false)
//                .Build();

//            var optionsBuilder = new DbContextOptionsBuilder<MyAppDbContext>();

//            optionsBuilder.UseSqlServer(
//                configuration.GetConnectionString("DefaultConnection"));

//            return new MyAppDbContext(optionsBuilder.Options);
//        }
//    }
//}



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