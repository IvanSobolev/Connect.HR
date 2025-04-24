namespace TinderAPI.Models.DTOs;

public class ProfileUpdateDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Description { get; set; } = null;
    public float? Latitude { get; set; } = null;
    public float? Longitude { get; set; } = null;
    public List<int>? HobbiesId { get; set; } = null;
}