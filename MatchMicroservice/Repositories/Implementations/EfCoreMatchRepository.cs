using MatchMicroservice.Models;
using MatchMicroservice.Models.Entities;
using MatchMicroservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MatchMicroservice.Repositories.Implementations;

public class EfCoreMatchRepository(DataContext dataContext) : IMatchRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<Match> UpsertMatch(Guid userId1, Guid userId2, bool? decision1, bool? decision2)
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
}