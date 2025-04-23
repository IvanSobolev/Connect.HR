using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;

namespace TinderAPI.Repositories.Interfaces;

public interface IProfileRepository
{
    Task AddAsync(Profile profile);
    Task<ProfileDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, 
        string? firstName = null, 
        string? lastName = null, 
        string? description = null, 
        float? latitude = null, 
        float? longitude = null);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistAsync(Guid id);
}