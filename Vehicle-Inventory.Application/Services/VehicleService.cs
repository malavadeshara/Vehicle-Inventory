//using Vehicle_Inventory.Application.Common;
//using Vehicle_Inventory.Application.Interfaces.Repositories;
//using Vehicle_Inventory.Application.Interfaces.Services;
//using Vehicle_Inventory.Domain.Entities;

//namespace Vehicle_Inventory.Infrastructure.Services
//{
//    public class VehicleService : IVehicleService
//    {
//        private readonly IVehicleRepository _vehicleRepository;

//        public VehicleService(IVehicleRepository vehicleRepository)
//        {
//            _vehicleRepository = vehicleRepository;
//        }

//        public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
//        {
//            return await _vehicleRepository.GetAllAsync();
//        }

//        public async Task<PagedResult<Vehicle>> GetPagedAsync(
//            int pageNumber,
//            int pageSize)
//        {
//            if (pageNumber <= 0)
//                throw new ArgumentException("PageNumber must be greater than 0");

//            if (pageSize <= 0 || pageSize > 100)
//                throw new ArgumentException("Invalid PageSize");

//            var (items, totalCount) =
//                await _vehicleRepository.GetPagedAsync(pageNumber, pageSize);

//            return PagedResult<Vehicle>.Create(
//                items,
//                pageNumber,
//                pageSize,
//                totalCount);
//        }

//        public async Task<Vehicle> GetByIdAsync(int id)
//        {
//            return await _vehicleRepository.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException("Vehicle not found");
//        }

//        public async Task<int> CreateAsync(
//            string name,
//            string model,
//            int year,
//            decimal price,
//            string currency)
//        {
//            var vehicle = new Vehicle(
//                name,
//                model,
//                year,
//                price,
//                currency);

//            await _vehicleRepository.AddAsync(vehicle);
//            return vehicle.Id;
//        }

//        public async Task<PagedResult<Vehicle>> GetFilteredAsync( bool? inStock, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
//        {
//            if (pageNumber <= 0)
//                throw new ArgumentException("PageNumber must be greater than 0");

//            if (pageSize <= 0 || pageSize > 100)
//                throw new ArgumentException("Invalid PageSize");

//            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
//                throw new ArgumentException("MinPrice cannot be greater than MaxPrice");

//            var (items, totalCount) =
//                await _vehicleRepository.GetFilteredPagedAsync(
//                    inStock,
//                    minPrice,
//                    maxPrice,
//                    pageNumber,
//                    pageSize
//                );

//            return PagedResult<Vehicle>.Create(
//                items,
//                pageNumber,
//                pageSize,
//                totalCount
//            );
//        }

//        // GET /api/vehicles/filter?inStock=true&minPrice=500000&maxPrice=1500000&pageNumber=1&pageSize=10

//        //public async Task UpdateAsync(
//        //    int id,
//        //    Action<Vehicle> updateAction)
//        //{
//        //    var vehicle = await _vehicleRepository.GetByIdAsync(id)
//        //        ?? throw new KeyNotFoundException("Vehicle not found");

//        //    updateAction(vehicle);
//        //    await _vehicleRepository.UpdateAsync(vehicle);
//        //}

//        public async Task UpdateAsync(int id, Func<Vehicle, Task> updateAction)
//        {
//            var vehicle = await _vehicleRepository.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException("Vehicle not found");

//            await updateAction(vehicle);

//            await _vehicleRepository.UpdateAsync(vehicle);
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var vehicle = await _vehicleRepository.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException("Vehicle not found");

//            await _vehicleRepository.DeleteAsync(vehicle);
//        }
//    }
//}


using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Application.Common;

namespace Vehicle_Inventory.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
        => await _vehicleRepository.GetAllAsync();

    public async Task<PagedResult<Vehicle>> GetPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ValidationException(ValidationErrorCode.PageNumberInvalid);

        if (pageSize <= 0 || pageSize > 100)
            throw new ValidationException(ValidationErrorCode.PageSizeInvalid);

        var (items, totalCount) = await _vehicleRepository.GetPagedAsync(pageNumber, pageSize);

        return PagedResult<Vehicle>.Create(items, pageNumber, pageSize, totalCount);
    }



    public async Task<Vehicle> GetByIdAsync(int id)
        => await _vehicleRepository.GetByIdAsync(id)
           ?? throw new ValidationException(ValidationErrorCode.VehicleNotFound);
    public async Task<Vehicle> GetVehicleDetailsByIdAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdWithDetailsAsync(id);

        if (vehicle == null)
            throw new ValidationException(ValidationErrorCode.VehicleNotFound);

        return vehicle;
    }
    public async Task<int> CreateAsync(string name, string model, int year, decimal price, string currency)
    {
        var vehicle = new Vehicle(name, model, year, price, currency);
        await _vehicleRepository.AddAsync(vehicle);
        return vehicle.Id;
    }

    public async Task<PagedResult<Vehicle>> GetFilteredAsync(bool? inStock, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ValidationException(ValidationErrorCode.PageNumberInvalid);

        if (pageSize <= 0 || pageSize > 100)
            throw new ValidationException(ValidationErrorCode.PageSizeInvalid);

        if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            throw new ValidationException(ValidationErrorCode.MinPriceGreaterThanMaxPrice);

        var (items, totalCount) = await _vehicleRepository.GetFilteredPagedAsync(inStock, minPrice, maxPrice, pageNumber, pageSize);

        return PagedResult<Vehicle>.Create(items, pageNumber, pageSize, totalCount);
    }

    public async Task UpdateAsync(int id, Func<Vehicle, Task> updateAction)
    {
        var vehicle = await _vehicleRepository.GetByIdWithDetailsAsync(id)
            ?? throw new ValidationException(ValidationErrorCode.VehicleNotFound);

        await updateAction(vehicle);
        await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id)
            ?? throw new ValidationException(ValidationErrorCode.VehicleNotFound);

        await _vehicleRepository.DeleteAsync(vehicle);
    }
}