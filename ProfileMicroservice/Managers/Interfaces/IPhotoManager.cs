using TinderAPI.Models.DTOs;

namespace TinderAPI.Managers.Interfaces;

public interface IPhotoManager
{
    Task<string> AddNewPhotoAsync(Guid id, IFormFile file);
    Task<PhotoDto?> GetByIdAsync(long id);
    Task<ICollection<PhotoDto>> GetUserPhotoAsync(Guid userId);
    Task<bool> UpdateAsync(long id, IFormFile? newFile = null, bool isMain = false);
    Task<bool> DeleteAsync(long id);
}