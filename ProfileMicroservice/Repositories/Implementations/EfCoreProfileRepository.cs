using Microsoft.EntityFrameworkCore;
using TinderAPI.Models;
using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;
using TinderAPI.Repositories.Interfaces;

namespace TinderAPI.Repositories.Implementations;

public class EfCoreProfileRepository(DataContext dataContext) : IProfileRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task AddAsync(Profile profile)
    {
        await _dataContext.AddAsync(profile);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<ProfileDto?> GetByIdAsync(Guid id)
    {
        return await _dataContext.Profiles.Select(p => new ProfileDto{
            Id = p.Id, 
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = (DateTime.Today - p.BirthdayDate.ToDateTime(new TimeOnly(0,0))).TotalDays / 365.25,
            IsMale = p.IsMale,
            Description = p.Description,
            LastActive = p.LastActive,
            Photos = p.Photos.Select(ph => new PhotoDto
            {
                PhotoId = ph.PhotoId,
                PhotoUrl = ph.PhotoUrl,
                IsMain = ph.IsMain
            }),
            Hobbies = p.Hobbies.Select(h => new HobbyDto
            {
                Id = h.Id,
                Name = h.Name
            }),
            Preferences = new ()
            {
                MinAge = p.Preferences.MinAge, 
                MaxAge = p.Preferences.MaxAge,
                RadiusKm = p.Preferences.RadiusKm
            }
        }).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> UpdateAsync(Guid id, string? firstName = null, string? lastName = null, string? description = null,
    float? latitude = null, float? longitude = null, List<int>? hobbiesId = null)
{
    var profile = await _dataContext.Profiles
        .Include(p => p.Hobbies)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (profile == null)
    {
        return false;
    }

    if (firstName != null) profile.FirstName = firstName;
    if (lastName != null) profile.LastName = lastName;
    if (description != null) profile.Description = description;

    if (latitude.HasValue && longitude.HasValue)
    {
        profile.Latitude = latitude.Value;
        profile.Longitude = longitude.Value;
        profile.LastActive = DateTime.UtcNow;
    }

    if (hobbiesId != null)
    {
        var existingHobbyIds = profile.Hobbies.Select(h => h.Id).ToList();
        var hobbiesToAddIds = hobbiesId.Except(existingHobbyIds).ToList();
        var hobbiesToRemove = profile.Hobbies.Where(h => !hobbiesId.Contains(h.Id)).ToList();

        if (hobbiesToAddIds.Any())
        {
            var hobbiesToAdd = await _dataContext.Hobbies
                .Where(h => hobbiesToAddIds.Contains(h.Id))
                .ToListAsync();

            foreach (var hobby in hobbiesToAdd)
            {
                profile.Hobbies.Add(hobby);
            }
        }

        foreach (var hobby in hobbiesToRemove)
        {
            profile.Hobbies.Remove(hobby);
        }
    }

    await _dataContext.SaveChangesAsync();
    return true;
}

    public async Task<bool> DeleteAsync(Guid id)
    {
        var profile = await _dataContext.Profiles.FindAsync(id);
    
        if (profile == null)
        {
            return false;
        }

        _dataContext.Profiles.Remove(profile);
        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await _dataContext.Profiles.AnyAsync(p => p.Id == id);
    }
}