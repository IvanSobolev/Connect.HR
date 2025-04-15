using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Models;

public class DataContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(m =>
        {
            m.HasKey(p => p.Id);
            m.HasMany(p => p.RefreshTokens)
                .WithOne(r => r.Profile);
        });
    }
}