using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<IReadOnlyList<Vehicle>> GetAllAsync();

        Task<(IReadOnlyList<Vehicle> Items, int TotalCount)>
            GetPagedAsync(int pageNumber, int pageSize);

        Task<(IReadOnlyList<Vehicle> Items, int TotalCount)>
            GetFilteredPagedAsync(
                bool? inStock,
                decimal? minPrice,
                decimal? maxPrice,
                int pageNumber,
                int pageSize);

        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle?> GetByIdWithDetailsAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
    }
}