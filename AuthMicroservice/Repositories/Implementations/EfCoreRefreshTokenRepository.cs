using AuthMicroservice.Models;
using AuthMicroservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Repositories.Implementations;

public class EfCoreRefreshTokenRepository (DataContext dataContext) : IRefreshTokenRepository
{
    private readonly DataContext _context = dataContext;
    
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.Profile)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task AddAsync(string token, DateTime expiresAt, Guid profileId)
    {
        await _context.RefreshTokens.AddAsync(new RefreshToken{Token = token, ExpiresAt = expiresAt, ProfileId = profileId});
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllForProfileAsync(Guid profileId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.ProfileId == profileId)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string token)
    {
        return await _context.RefreshTokens.AnyAsync(rt => rt.Token == token);
    }
}