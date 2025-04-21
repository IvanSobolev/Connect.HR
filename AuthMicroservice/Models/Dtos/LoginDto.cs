using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models.Dtos;

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}