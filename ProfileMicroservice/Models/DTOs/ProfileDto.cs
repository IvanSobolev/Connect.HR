namespace TinderAPI.Models.DTOs;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double Age { get; set; }
    public bool IsMale { get; set; }
    public string? Description { get; set; }
    public DateTime LastActive { get; set; }
    public IEnumerable<PhotoDto> Photos { get; set; }
    public IEnumerable<HobbyDto> Hobbies { get; set; }
    public PreferencesDto Preferences { get; set; }
}