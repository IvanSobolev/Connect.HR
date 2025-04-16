using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models.Dtos;

public class RegistrationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsVerified { get; set; }
    public bool IsMale { get; set; }
    public DateOnly BirthdayDate { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}