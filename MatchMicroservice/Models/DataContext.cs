using MatchMicroservice.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchMicroservice.Models;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>().HasKey(m => new { m.UserId1, m.UserId2 });
    }
}