//using Microsoft.EntityFrameworkCore;
//using Vehicle_Inventory.Application.Interfaces.Repositories;
//using Vehicle_Inventory.Domain.Entities;
//using Vehicle_Inventory.Infrastructure.Data;

//namespace Vehicle_Inventory.Infrastructure.Repositories
//{
//    public class VehicleRepository : IVehicleRepository
//    {
//        private readonly MyAppDbContext _context;

//        public VehicleRepository(MyAppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
//        {
//            return await _context.Vehicles
//                .AsNoTracking()
//                .ToListAsync();
//        }

//        public async Task<(IReadOnlyList<Vehicle>, int)> GetPagedAsync(int pageNumber, int pageSize)
//        {
//            var query = _context.Vehicles.AsNoTracking();

//            var totalCount = await query.CountAsync();

//            var items = await query
//                .OrderBy(v => v.Id)
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            return (items, totalCount);
//        }

//        public async Task<(IReadOnlyList<Vehicle>, int)> GetFilteredPagedAsync(bool? inStock, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
//        {
//            IQueryable<Vehicle> query = _context.Vehicles.AsNoTracking();

//            if (inStock.HasValue)
//                query = query.Where(v => v.InStock == inStock.Value);

//            if (minPrice.HasValue)
//                query = query.Where(v => v.Price >= minPrice.Value);

//            if (maxPrice.HasValue)
//                query = query.Where(v => v.Price <= maxPrice.Value);

//            var totalCount = await query.CountAsync();

//            var items = await query
//                .OrderBy(v => v.Id)
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            return (items, totalCount);
//        }

//        public async Task<Vehicle?> GetByIdAsync(int id)
//        {
//            return await _context.Vehicles
//                .FirstOrDefaultAsync(v => v.Id == id);
//        }

//        public async Task AddAsync(Vehicle vehicle)
//        {
//            await _context.Vehicles.AddAsync(vehicle);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(Vehicle vehicle)
//        {
//            _context.Vehicles.Update(vehicle);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(Vehicle vehicle)
//        {
//            _context.Vehicles.Remove(vehicle);
//            await _context.SaveChangesAsync();
//        }
//    }
//}



using Microsoft.EntityFrameworkCore;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Infrastructure.Data;
using Vehicle_Inventory.Infrastructure.Exceptions;

namespace Vehicle_Inventory.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MyAppDbContext _context;

        public VehicleRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
        {
            try
            {
                return await _context.Vehicles
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<(IReadOnlyList<Vehicle>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.Vehicles.Include(v => v.Images).AsNoTracking();
                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(v => v.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<(IReadOnlyList<Vehicle>, int)> GetPagedWithDetailsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.Vehicles
                    .Include(v => v.Images)
                    .Include(v => v.Features)
                    .Include(v => v.Dimensions)
                    .Include(v => v.Specifications)
                    .AsNoTracking();

                var totalCount = await query.CountAsync();

                var items = await query
                    .OrderBy(v => v.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(
                    InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }


        public async Task<(IReadOnlyList<Vehicle>, int)> GetFilteredPagedAsync(bool? inStock, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<Vehicle> query = _context.Vehicles.Include(v => v.Images).AsNoTracking();

                if (inStock.HasValue)
                    query = query.Where(v => v.InStock == inStock.Value);

                if (minPrice.HasValue)
                    query = query.Where(v => v.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(v => v.Price <= maxPrice.Value);

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(v => v.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<Vehicle?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.Vehicles
                    .Include(v => v.Images)
                    .Include(v => v.Features)
                    .Include(v => v.Dimensions)
                    .Include(v => v.Specifications)
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(
                    InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            try
            {
                await _context.Vehicles.AddAsync(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }
    }
}