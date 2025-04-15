using AuthMicroservice.Models;
using AuthMicroservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Repositories.Implementations;

public class EfCoreProfileRepository (DataContext context) : IProfileRepository
{
    private readonly DataContext _context = context;
    
    public async Task AddAsync(Guid id, string email, string passwordHash)
    {
        await _context.Profiles.AddAsync(new Profile
        {
            Id = id,
            Email = email,
            PasswordHash = passwordHash
        });
        await _context.SaveChangesAsync();
    }

    public async Task<Profile?> GetByIdAsync(Guid id)
    {
        return await _context.Profiles
            .Include(p => p.RefreshTokens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Profile?> GetByEmailAsync(string email)
    {
        return await _context.Profiles
            .FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task UpdateAsync(Guid id, string? email = null, string? passwordHash = null, bool? isVerified = null)
    {
        var profile = await GetByIdAsync(id);

        if (profile == null)
        {
            return;
        }

        profile.Email = email ?? profile.Email;
        profile.PasswordHash = passwordHash ?? profile.PasswordHash;
        profile.IsVerified = isVerified ?? profile.IsVerified;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var profile = await GetByIdAsync(id);
        if (profile != null)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Profiles.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Profiles.AnyAsync(p => p.Email == email);
    }
}