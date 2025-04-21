using System.ComponentModel.DataAnnotations;

namespace TinderAPI.Models.Entitys;

public class Profile
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public bool IsMale { get; set; }
    public DateOnly BirthdayDate { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    
    public ICollection<Hobby> Hobbies { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public Preferences Preferences { get; set; }
}