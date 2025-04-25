using MatchMicroservice.Models.Entities;

namespace MatchMicroservice.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<Match> UpsertMatch(Guid userId1, Guid userId2, bool? decision1, bool? decision2);
}