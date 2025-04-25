using MatchMicroservice.Models.Entities;

namespace MatchMicroservice.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<Swipe> UpsertSwipeAsync(Guid userId1, Guid userId2, bool? decision1, bool? decision2);
    Task<ICollection<Swipe>> GetMatchAsync(Guid userId, int page, int pageSize);
    Task<ICollection<Swipe>> GetByIdAsync(Guid userId, int page, int pageSize);
}