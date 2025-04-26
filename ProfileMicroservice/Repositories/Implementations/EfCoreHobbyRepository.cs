using Microsoft.EntityFrameworkCore;
using TinderAPI.Models;
using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;
using TinderAPI.Repositories.Interfaces;

namespace TinderAPI.Repositories.Implementations;

public class EfCoreHobbyRepository(DataContext context) : IHobbyRepository
{
    private readonly DataContext _context = context;
    public async Task AddAsync(string name)
    {
        if (await _context.Hobbies.AnyAsync(h => h.Name == name))
        {
            return;
        }

        var hobby = new Hobby() { Name = name.Trim() };
        await _context.Hobbies.AddAsync(hobby);
        await _context.SaveChangesAsync();
    }

    public async Task<HobbyDto?> GetByIdAsync(int id)
    {
        return await _context.Hobbies
            .Where(h => h.Id == id)
            .Select(h => new HobbyDto
            {
                Id = h.Id,
                Name = h.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<HobbyDto>> GetAll()
    {
        return await _context.Hobbies
            .OrderBy(h => h.Name)
            .Select(h => new HobbyDto
            {
                Id = h.Id,
                Name = h.Name
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(int id, string name)
    {
        var hobby = await _context.Hobbies.FindAsync(id);
        if (hobby == null) 
        { 
            return false;
        }

        if (await _context.Hobbies.AnyAsync(h => h.Name == name && h.Id != id)) 
        { 
            return false;
        }

        hobby.Name = name.Trim();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hobby = await _context.Hobbies
            .Include(h => h.Profiles)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hobby == null)
        {
            return false;
        }

        if (hobby.Profiles.Any())
        { 
            return false;
        }

        _context.Hobbies.Remove(hobby);
        await _context.SaveChangesAsync();
        return true;
    }
}