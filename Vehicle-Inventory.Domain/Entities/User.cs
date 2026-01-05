using Vehicle_Inventory.Domain.Exceptions;

namespace Vehicle_Inventory.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiry { get; private set; }

    protected User() { } // For EF Core

    public User(string userName, string email)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException(DomainErrorCode.UserNameRequired);

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException(DomainErrorCode.EmailRequired);

        //if (string.IsNullOrWhiteSpace(passwordHash))
        //    throw new DomainException(DomainErrorCode.PasswordRequired);

        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = null!;
        Role = UserRole.Customer;
    }

    public void SetRefreshToken(string token, DateTime expiry)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException(DomainErrorCode.InvalidRefreshToken);

        if (expiry <= DateTime.UtcNow)
            throw new DomainException(DomainErrorCode.RefreshTokenExpiryInvalid);

        RefreshToken = token;
        RefreshTokenExpiry = expiry;
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiry = null;
    }
}