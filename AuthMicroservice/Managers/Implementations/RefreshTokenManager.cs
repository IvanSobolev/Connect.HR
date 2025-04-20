using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Repositories.Interfaces;
using AuthMicroservice.Services.Interfaces;

namespace AuthMicroservice.Managers.Implementations;

public class RefreshTokenManager( IRefreshTokenRepository refreshTokenRepository, ITokenGeneratorService tokenGenerator, ILogger logger) : IRefreshTokenManager
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly ITokenGeneratorService _tokenGenerator = tokenGenerator;
    private readonly ILogger _logger = logger;
    
    public async Task<TokensDto> RefreshTokensAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null)
        {
            _logger.LogWarning("Token not found for Refresh tokens");
            return new TokensDto();
        }
        await _refreshTokenRepository.DeleteAsync(refreshToken);
        if (token.ExpiresAt > DateTime.UtcNow)
        {
            _logger.LogDebug("Token is expires for Refresh tokens");
            return new TokensDto();
        }
        
        string newRefreshToken = await _tokenGenerator.GenerateTokenAsync(token.ProfileId, DateTime.UtcNow.AddDays(30));
        string newAccessToken = await _tokenGenerator.GenerateTokenAsync(token.ProfileId, DateTime.UtcNow.AddMinutes(15));
        await _refreshTokenRepository.AddAsync(newRefreshToken, DateTime.UtcNow.AddDays(30), token.ProfileId);
        return new TokensDto(newAccessToken, newRefreshToken);
    }

    public async Task LogoutTokenAsync(string refreshToken)
    {
        await _refreshTokenRepository.DeleteAsync(refreshToken);
    }

    public async Task LogoutAllTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null)
        {
            _logger.LogWarning("Token not found for logout all tokens");
            return;
        }
        await _refreshTokenRepository.DeleteAsync(refreshToken);
        if (token.ExpiresAt > DateTime.UtcNow)
        {
            _logger.LogDebug("Token is expires for logout all tokens");
            return;
        }

        await _refreshTokenRepository.DeleteAllForProfileAsync(token.ProfileId);
    }
}