using TinderAPI.Models.DTOs;

namespace TinderAPI.Managers.Interfaces;

public interface IProfileManager
{
    Task<ICollection<ProfileDto>> GetDeckForUserAsync(Guid id);
    Task<ProfileDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(ProfileUpdateDto profileUpdateDto);
}