using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;

namespace TinderAPI.Repositories.Interfaces;

public interface IPhotoRepository
{
    Task AddNewPhotoAsync(Guid id, string url);
    Task<PhotoDto?> GetByIdAsync(long id);
    Task<ICollection<PhotoDto>> GetUsersPhotoAsync(Guid userId);
    Task<bool> UpdateAsync(long id, string? url = null, bool isMain = false);
    Task<bool> DeleteAsync(long id);
}