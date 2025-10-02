using Microsoft.EntityFrameworkCore;
using TinderAPI.Models.Entitys;

namespace TinderAPI.Models;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<Preferences> Preferences { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(m =>
        {
            m.HasKey(p => p.Id);
            m.HasIndex(p => new { p.Latitude, p.Longitude});
            m.HasIndex(p => new { p.BirthdayDate, p.IsMale });
            m.HasMany(p => p.Hobbies)
                .WithMany(h => h.Profiles);
        });
        
        modelBuilder.Entity<Preferences>(m =>
        {
            m.HasKey(pr => pr.ProfileId);
            m.HasIndex(pr => new { pr.MinAge, pr.MaxAge, Radius = pr.RadiusKm });
            m.HasOne(pr => pr.Profile)
                .WithOne(p => p.Preferences)
                .HasForeignKey<Preferences>(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
                
        });
            
        modelBuilder.Entity<Photo>(m =>
        {
            m.HasIndex(p => p.ProfileId);
            m.HasOne(p => p.Profile)
                .WithMany(p => p.Photos)
                .HasForeignKey(p => p.ProfileId);
        });
    }
}