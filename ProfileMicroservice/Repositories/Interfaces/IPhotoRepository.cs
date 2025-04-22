using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;

namespace TinderAPI.Repositories.Interfaces;

public interface IPhotoRepository
{
    Task AddNewPhotoAsync(Guid id, string url, int? position = null);
    Task<PhotoDto> GetByIdAsync(long id);
    Task<ICollection<PhotoDto>> GetUsersPhotoAsync(Guid userId);
    Task UpdateAsync(long id, string? url = null, int? position = null);
    Task DeleteAsync(long id);
}