using TinderAPI.Models.DTOs;

namespace TinderAPI.Repositories.Interfaces;

public interface IHobbyRepository
{
    Task AddAsync(string name);
    Task<HobbyDto> GetByIdAsync(int id);
    Task<ICollection<HobbyDto>> GetAll();
    Task UpdateAsync(int id, string name);
    Task DeleteAsync(int id);
}