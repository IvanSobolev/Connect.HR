using System.ComponentModel.DataAnnotations;

namespace DeckCollectorMicroservice.Models.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsMale { get; set; }
    public DateOnly BirthdayDate { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public ICollection<Hobby> Hobbies { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public Preferences Preferences { get; set; }
}