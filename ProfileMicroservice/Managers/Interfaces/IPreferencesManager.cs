using TinderAPI.Models.DTOs;

namespace TinderAPI.Managers.Interfaces;

public interface IPreferencesManager
{
    Task<bool> UpdateAsync(PreferencesUpdateDto preferencesUpdateDto);
}