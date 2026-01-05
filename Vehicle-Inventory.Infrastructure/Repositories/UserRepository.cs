//using Microsoft.EntityFrameworkCore;
//using Vehicle_Inventory.Application.Interfaces.Repositories;
//using Vehicle_Inventory.Domain.Entities;
//using Vehicle_Inventory.Infrastructure.Data;

//namespace Vehicle_Inventory.Infrastructure.Repositories
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly MyAppDbContext _context;

//        public UserRepository(MyAppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<bool> ExistsAsync(string username, string email)
//        {
//            return await _context.Users
//                .AnyAsync(u => u.UserName == username || u.Email == email);
//        }

//        public async Task<User?> GetByEmailAsync(string email)
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.Email == email);
//        }

//        public async Task<User?> GetByIdAsync(Guid id)
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.Id == id);
//        }

//        public async Task AddAsync(User user)
//        {
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(User user)
//        {
//            _context.Users.Update(user);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
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
    public class UserRepository : IUserRepository
    {
        private readonly MyAppDbContext _context;

        public UserRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            try
            {
                return await _context.Users
                    .AnyAsync(u => u.UserName == username || u.Email == email);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message); // Exception debugging suggested by rahul sir
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }

        public async Task AddAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseUpdateFailed, ex);
            }
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(InfrastructureErrorCode.DatabaseOperationFailed, ex);
            }
        }
    }
}