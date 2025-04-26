using Microsoft.EntityFrameworkCore;
using TinderAPI.Models;
using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;
using TinderAPI.Repositories.Interfaces;

namespace TinderAPI.Repositories.Implementations;

public class EfCorePreferencesRepository (DataContext dataContext) : IPreferencesRepository
{
    private readonly DataContext _dataContext = dataContext;
    public async Task AddForNewUser(Guid id, int userAge, float radius = 10)
    {
        var preferences = new Preferences()
        {
            ProfileId = id,
            MinAge = Math.Max(18, userAge - 5), // Пример логики: ±5 лет от возраста
            MaxAge = userAge + 5,
            RadiusKm = radius
        };

        await _dataContext.Preferences.AddAsync(preferences);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<PreferencesDto?> GetByIdAsync(Guid id)
    {
        return await _dataContext.Preferences
            .Where(p => p.ProfileId == id)
            .Select(p => new PreferencesDto
            {
                MinAge = p.MinAge,
                MaxAge = p.MaxAge,
                RadiusKm = p.RadiusKm
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, int? minAge = null, int? maxAge = null, float? radius = null)
    {
        var preferences = await _dataContext.Preferences
            .FirstOrDefaultAsync(p => p.ProfileId == id);

        if (preferences == null)
        {
            return false;
        }

        if (minAge.HasValue) preferences.MinAge = minAge.Value;
        if (maxAge.HasValue) preferences.MaxAge = maxAge.Value;
        if (radius.HasValue) preferences.RadiusKm = radius.Value;

        await _dataContext.SaveChangesAsync();
        return true;
    }
}