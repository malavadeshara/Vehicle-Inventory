namespace Vehicle_Inventory.Application.DTOs.Auth;

public class TokenResponseDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }

    public TokenResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}