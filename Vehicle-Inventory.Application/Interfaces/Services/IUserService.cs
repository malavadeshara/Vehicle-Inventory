using Vehicle_Inventory.Domain.Entities;

namespace Vehicle_Inventory.Application.Interfaces.Services;

public interface IUserService
{
    Task RegisterAsync(string userName, string email, string password);
    Task<User> getByIdAsync(Guid userId);
}