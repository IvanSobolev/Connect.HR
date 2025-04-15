using AuthMicroservice.Models;

namespace AuthMicroservice.Repositories.Interfaces;

public interface IProfileRepository
{
    Task AddAsync(Guid id, string email, string passwordHash);
    Task<Profile?> GetByIdAsync(Guid id);
    Task<Profile?> GetByEmailAsync(string email);
    Task UpdateAsync(Guid id, string? email = null, string? passwordHash = null, bool? isVerified = null);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email);
}