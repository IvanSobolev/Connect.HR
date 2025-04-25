using MatchMicroservice.Manager.Interfaces;
using MatchMicroservice.Models.Entities;
using MatchMicroservice.Repositories.Interfaces;

namespace MatchMicroservice.Manager.Implementations;

public class SwipeManager (ISwipeRepository swipeRepository) : ISwipeManager
{
    private readonly ISwipeRepository _swipeRepository = swipeRepository;
    
    public async Task<Swipe> RegisterDecision(DecisionDto decision)
    {
        return await _swipeRepository.UpsertSwipeAsync(decision.UserA, decision.UserB, decision.DecisionA, decision.DecisionB);
    }

    public async Task<ICollection<Swipe>> GetMatchAsync(Guid userId, int page, int pageSize)
    {
        return await _swipeRepository.GetMatchAsync(userId, page, pageSize);
    }

    public async Task<ICollection<Swipe>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        return await _swipeRepository.GetByUserIdAsync(userId, page, pageSize);
    }
}