namespace TinderAPI.Models.DTOs;

public class PreferencesUpdateDto
{
    public Guid Id { get; set; }
    public int? MinAge { get; set; } = null;
    public int? MaxAge { get; set; } = null;
    public float? Radius { get; set; } = null;
}