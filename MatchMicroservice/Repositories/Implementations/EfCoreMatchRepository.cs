using MatchMicroservice.Models;
using MatchMicroservice.Models.Entities;
using MatchMicroservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MatchMicroservice.Repositories.Implementations;

public class EfCoreMatchRepository(DataContext dataContext) : IMatchRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<Swipe> UpsertSwipeAsync(Guid userId1, Guid userId2, bool? decision1, bool? decision2)
    {
        var sql = @"
INSERT INTO ""Matches"" (
    ""UserId1"", ""UserId2"", ""DecisionId1"", ""DecisionId2"", ""CreatedAt"", ""UpdatedAt""
)
VALUES (
    {0}, {1}, {2}, {3}, NOW(), NOW()
)
ON CONFLICT (""UserId1"", ""UserId2"")
DO UPDATE SET
    ""DecisionId1"" = COALESCE(EXCLUDED.""DecisionId1"", ""Matches"".""DecisionId1""),
    ""DecisionId2"" = COALESCE(EXCLUDED.""DecisionId2"", ""Matches"".""DecisionId2""),
    ""UpdatedAt"" = NOW()
RETURNING *;";

        return await _dataContext.Matches
            .FromSqlRaw(sql, userId1, userId2, decision1, decision2)
            .AsNoTracking()
            .FirstAsync();
    }

    public async Task<ICollection<Swipe>> GetMatchAsync(Guid userId, int page, int pageSize)
    {
        if (page > 0 && pageSize > 0)
        {
            return new List<Swipe>();
        }
        var queryable = _dataContext.Matches.AsQueryable();
        queryable = queryable.Where(s => (s.UserId1 == userId || s.UserId2 == userId) && s.DecisionId1 == true && s.DecisionId2 == true);
        queryable = queryable.Skip(pageSize * page).Take(pageSize);
        return await queryable.ToListAsync();
    }
    
    public async Task<ICollection<Swipe>> GetByIdAsync(Guid userId, int page, int pageSize)
    {
        if (page > 0 && pageSize > 0)
        {
            return new List<Swipe>();
        }
        var queryable = _dataContext.Matches.AsQueryable();
        queryable = queryable.Where(s => s.UserId1 == userId || s.UserId2 == userId);
        queryable = queryable.Skip(pageSize * page).Take(pageSize);
        return await queryable.ToListAsync();
    }
}