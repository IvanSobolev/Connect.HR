namespace DeckCollectorMicroservice.Models.DTOs;

public class ProfileSearchDto
{
    public Guid Id { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsMale { get; set; }
    public int Age { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public float RadiusKm { get; set; }
    public ICollection<HobbyDto> Hobbies { get; set; }
}