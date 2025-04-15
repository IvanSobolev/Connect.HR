using System.ComponentModel.DataAnnotations;

namespace TinderAPI.Models.DTOs;

public class CreateProfileDto
{
    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    public string Password { get; set; }
        
    public bool IsMale { get; set; }
    public DateOnly BirthdayDate { get; set; }
    public string? Description { get; set; }
}