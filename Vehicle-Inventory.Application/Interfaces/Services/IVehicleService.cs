using Vehicle_Inventory.Application.Common;
using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<IReadOnlyList<Vehicle>> GetAllAsync();

        Task<PagedResult<Vehicle>> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<PagedResult<Vehicle>> GetFilteredAsync(
            bool? inStock,
            decimal? minPrice,
            decimal? maxPrice,
            int pageNumber,
            int pageSize);

        Task<Vehicle> GetByIdAsync(int id);
        Task<Vehicle> GetVehicleDetailsByIdAsync(int id);
        Task<int> CreateAsync(
            string name,
            string model,
            int year,
            decimal price,
            string currency);

        //Task UpdateAsync(int id, Action<Vehicle> updateAction);

        Task UpdateAsync(int id, Func<Vehicle, Task> updateAction);

        Task DeleteAsync(int id);
    }
}