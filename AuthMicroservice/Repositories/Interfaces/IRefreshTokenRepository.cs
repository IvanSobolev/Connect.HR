using AuthMicroservice.Models;

namespace AuthMicroservice.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task AddAsync(string token, DateTime expiresAt, Guid profileId);
    Task DeleteAsync(string token);
    Task DeleteAllForProfileAsync(Guid profileId);
    Task<bool> ExistsAsync(string token);
}