using TinderAPI.Models.DTOs;

namespace TinderAPI.Repositories.Interfaces;

public interface IPreferencesRepository
{
    Task AddForNewUser(Guid id, int userAge, float radius = 10);
    Task<PreferencesDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, int? minAge = null, int? maxAge = null, float? radius = null);
}