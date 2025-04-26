using TinderAPI.Models.DTOs;

namespace TinderAPI.Managers.Interfaces;

public interface IHobbyManager
{
    Task AddAsync(string name);
    Task<HobbyDto?> GetByIdAsync(int id);
    Task<ICollection<HobbyDto>> GetAll();
    Task<bool> UpdateAsync(int id, string name);
    Task<bool> DeleteAsync(int id);
}