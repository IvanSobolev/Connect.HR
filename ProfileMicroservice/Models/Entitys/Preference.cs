namespace TinderAPI.Models.Entitys;

public class Preference
{
    public Guid UserId { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public float Radius { get; set; }
}