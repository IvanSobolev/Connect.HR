using AuthMicroservice.Models.Dtos;

namespace AuthMicroservice.Managers.Interfaces;

public interface IRefreshTokenManager
{
    Task<TokensDto> RefreshTokensAsync(string refreshToken);
    Task LogoutTokenAsync(string refreshToken);
    Task LogoutAllTokenAsync(string refreshToken);
}