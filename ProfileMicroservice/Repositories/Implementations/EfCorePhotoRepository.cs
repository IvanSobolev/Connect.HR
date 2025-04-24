using Microsoft.EntityFrameworkCore;
using TinderAPI.Models;
using TinderAPI.Models.DTOs;
using TinderAPI.Models.Entitys;
using TinderAPI.Repositories.Interfaces;

namespace TinderAPI.Repositories.Implementations;

public class EfCorePhotoRepository(DataContext dataContext) : IPhotoRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task AddNewPhotoAsync(Guid id, string url)
    {
        bool isFirstPhoto = !await _dataContext.Photos.AnyAsync(p => p.ProfileId == id);

        var photo = new Photo
        {
            ProfileId = id,
            PhotoUrl = url,
            IsMain = isFirstPhoto
        };

        await _dataContext.Photos.AddAsync(photo);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<PhotoDto?> GetByIdAsync(long id)
    {
        return await _dataContext.Photos
            .Where(p => p.PhotoId == id)
            .Select(p => new PhotoDto
            {
                PhotoId = p.PhotoId,
                PhotoUrl = p.PhotoUrl,
                IsMain = p.IsMain
            }).FirstOrDefaultAsync();
    }

    public async Task<ICollection<PhotoDto>> GetUserPhotoAsync(Guid userId)
    {
        return await _dataContext.Photos
            .Where(p => p.ProfileId == userId)
            .Select(p => new PhotoDto
            {
                PhotoId = p.PhotoId,
                PhotoUrl = p.PhotoUrl,
                IsMain = p.IsMain
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(long id, string? url = null, bool isMain = false)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var photo = await _dataContext.Photos.FindAsync(id);
            if (photo == null) return false;
            
            if (!string.IsNullOrWhiteSpace(url))
                photo.PhotoUrl = url;
            
            if (isMain)
            {
                await _dataContext.Photos
                    .Where(p => p.ProfileId == photo.ProfileId)
                    .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsMain, false));

                photo.IsMain = true;
            }

            await _dataContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
             Console.WriteLine(ex +  "Failed to update photo {PhotoId}" +  id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var photo = await _dataContext.Photos.FindAsync(id);
            if (photo == null) return false;

            bool wasMain = photo.IsMain;
            _dataContext.Photos.Remove(photo);
            await _dataContext.SaveChangesAsync();

            if (wasMain)
            {
                var newMainPhoto = await _dataContext.Photos
                    .Where(p => p.ProfileId == photo.ProfileId)
                    .FirstOrDefaultAsync();

                if (newMainPhoto != null)
                {
                    newMainPhoto.IsMain = true;
                    await _dataContext.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex + "Failed to delete photo {PhotoId}" +  id);
            return false;
        }
    }
}