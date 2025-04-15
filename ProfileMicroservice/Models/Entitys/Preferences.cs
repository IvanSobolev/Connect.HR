namespace TinderAPI.Models.Entitys;

public class Preferences
{
    public Guid ProfileId { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public float RadiusKm { get; set; }
    
    public Profile Profile { get; set; }
}