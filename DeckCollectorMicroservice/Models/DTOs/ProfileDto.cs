using System.Text.RegularExpressions;

namespace DeckCollectorMicroservice.Models.DTOs;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsMale { get; set; }
    public int Age { get; set; }
    public string? Description { get; set; }
    public double DistanceKm { get; set; }
    public ICollection<HobbyDto> Hobbies { get; set; }
    public ICollection<PhotoDto> Photos { get; set; }
}