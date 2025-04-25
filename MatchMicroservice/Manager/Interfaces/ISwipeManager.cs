using MatchMicroservice.Models.Entities;

namespace MatchMicroservice.Manager.Interfaces;

public interface ISwipeManager
{
    Task<Swipe> RegisterDecision(DecisionDto decision);
    Task<ICollection<Swipe>> GetMatchAsync(Guid userId, int page, int pageSize);
    Task<ICollection<Swipe>> GetByUserIdAsync(Guid userId, int page, int pageSize);
}