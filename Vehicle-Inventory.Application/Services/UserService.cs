//using Vehicle_Inventory.Application.Interfaces.Repositories;
//using Vehicle_Inventory.Application.Interfaces.Services;
//using Vehicle_Inventory.Domain.Entities;
//using Microsoft.AspNetCore.Identity;

//namespace Vehicle_Inventory.Infrastructure.Services;

//public class UserService : IUserService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IPasswordHasher<User> _passwordHasher;

//    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
//    {
//        _userRepository = userRepository;
//        _passwordHasher = passwordHasher;
//    }

//    public async Task RegisterAsync(string userName, string email, string password)
//    {
//        var exists = await _userRepository.ExistsAsync(userName, email);
//        if (exists)
//            throw new InvalidOperationException("User already exists");

//        var user = new User(userName, email, string.Empty);

//        var passwordHash = _passwordHasher.HashPassword(user, password);
//        typeof(User)
//            .GetProperty(nameof(User.PasswordHash))!
//            .SetValue(user, passwordHash);

//        await _userRepository.AddAsync(user);
//    }

//    public async Task<User> getByIdAsync(Guid userId)
//    {
//        var user = await _userRepository.GetByIdAsync(userId);
//        if(user is null)
//        {
//            throw new InvalidCastException("User not found");
//        }
//        return user;
//    }
//}


using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Application.Interfaces.Repositories;
using Vehicle_Inventory.Application.Interfaces.Services;
using Vehicle_Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Vehicle_Inventory.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterAsync(string userName, string email, string password)
    {
        Console.WriteLine("Inside user service", userName);
        var exists = await _userRepository.ExistsAsync(userName, email);
        if (exists)
            throw new ValidationException(ValidationErrorCode.UserAlreadyExists);

        Console.WriteLine("User does not exists, proceeding to create user");

        var user = new User(userName, email);

        Console.WriteLine("User object created");

        var passwordHash = _passwordHasher.HashPassword(user, password);
        typeof(User).GetProperty(nameof(User.PasswordHash))!.SetValue(user, passwordHash);

        await _userRepository.AddAsync(user);
    }

    public async Task<User> getByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new ValidationException(ValidationErrorCode.UserNotFound);

        return user;
    }
}