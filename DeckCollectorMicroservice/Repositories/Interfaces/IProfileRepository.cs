using DeckCollectorMicroservice.Models.DTOs;

namespace DeckCollectorMicroservice.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<ICollection<ProfileDto>> GetProfileDeckForUser(ProfileSearchDto profile, int page, int pageSize);
    Task<ICollection<ProfileSearchDto>> GetAllActiveUser();
    Task<ProfileSearchDto> GetUserById(Guid id);
}