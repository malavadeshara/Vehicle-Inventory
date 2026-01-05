using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Interfaces.Repositories;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsAsync(string username, string email);

    Task AddAsync(User user);
    Task UpdateAsync(User user);

    // (refresh token related)
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}