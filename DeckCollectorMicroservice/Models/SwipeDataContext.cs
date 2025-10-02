using DeckCollectorMicroservice.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeckCollectorMicroservice.Models;

public class SwipeDataContext(DbContextOptions<SwipeDataContext> options) : DbContext(options)
{
    public DbSet<Swipe> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Swipe>().HasKey(m => new { m.UserId1, m.UserId2 }); 
    }
}