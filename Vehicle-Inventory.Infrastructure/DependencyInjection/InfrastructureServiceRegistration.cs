using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Infrastructure.Repositories;
using Vehicle_Inventory.Infrastructure.Services;

namespace Vehicle_Inventory.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();

            return services;
        }
    }
}