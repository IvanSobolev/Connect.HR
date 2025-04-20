using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Models;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(m =>
        {
            m.HasKey(r => r.Token);
        });
        modelBuilder.Entity<Profile>(m =>
        {
            m.HasKey(p => p.Id);
            m.HasMany(p => p.RefreshTokens)
                .WithOne(r => r.Profile)
                .HasForeignKey(r => r.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}