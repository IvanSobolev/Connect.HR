using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models;

public class Profile
{
    public Guid Id { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsVerified { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}