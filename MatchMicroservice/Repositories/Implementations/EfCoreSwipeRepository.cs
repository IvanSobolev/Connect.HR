using MatchMicroservice.Models;
using MatchMicroservice.Models.Entities;
using MatchMicroservice.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MatchMicroservice.Repositories.Implementations;

public class EfCoreSwipeRepository(DataContext dataContext) : ISwipeRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<Swipe> UpsertSwipeAsync(Guid userId1, Guid userId2, bool? decision1, bool? decision2)
    {
        await using var conn = _dataContext.Database.GetDbConnection();
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
INSERT INTO ""Matches"" (
    ""UserId1"", ""UserId2"", ""DecisionId1"", ""DecisionId2"", ""CreatedAt"", ""UpdatedAt""
)
VALUES (
    @user1, @user2, @decision1, @decision2, NOW(), NOW()
)
ON CONFLICT (""UserId1"", ""UserId2"")
DO UPDATE SET
    ""DecisionId1"" = COALESCE(EXCLUDED.""DecisionId1"", ""Matches"".""DecisionId1""),
    ""DecisionId2"" = COALESCE(EXCLUDED.""DecisionId2"", ""Matches"".""DecisionId2""),
    ""UpdatedAt"" = NOW()
RETURNING *;";

        cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@user1", userId1));
        cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@user2", userId2));
        cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@decision1", decision1 ?? (object)DBNull.Value));
        cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@decision2", decision2 ?? (object)DBNull.Value));

        await using var reader = await cmd.ExecuteReaderAsync();
    
        if (await reader.ReadAsync())
        {
            return new Swipe()
            {
                UserId1 = reader.GetGuid(reader.GetOrdinal("UserId1")),
                UserId2 = reader.GetGuid(reader.GetOrdinal("UserId2")),
                DecisionId1 = reader.IsDBNull(reader.GetOrdinal("DecisionId1")) ? null : reader.GetBoolean(reader.GetOrdinal("DecisionId1")),
                DecisionId2 = reader.IsDBNull(reader.GetOrdinal("DecisionId2")) ? null : reader.GetBoolean(reader.GetOrdinal("DecisionId2")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        throw new Exception("Insert or update failed");
    }

    public async Task<ICollection<Swipe>> GetMatchAsync(Guid userId, int page, int pageSize)
    {
        if (page < 0 || pageSize <= 0)
        {
            return new List<Swipe>();
        }

        var queryable = _dataContext.Matches.AsQueryable();
        queryable = queryable.Where(s => (s.UserId1 == userId || s.UserId2 == userId) && s.DecisionId1 == true && s.DecisionId2 == true);
        queryable = queryable.Skip(pageSize * page).Take(pageSize);
        return await queryable.ToListAsync();
    }

    public async Task<ICollection<Swipe>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        if (page < 0 || pageSize <= 0)
        {
            return new List<Swipe>();
        }

        var queryable = _dataContext.Matches.AsQueryable();
        queryable = queryable.Where(s => s.UserId1 == userId || s.UserId2 == userId);
        queryable = queryable.Skip(pageSize * page).Take(pageSize);
        return await queryable.ToListAsync();
    }
}