using Microsoft.Extensions.DependencyInjection;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Application.Services;

namespace Vehicle_Inventory.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVehicleService, VehicleService>();

            return services;
        }
    }
}