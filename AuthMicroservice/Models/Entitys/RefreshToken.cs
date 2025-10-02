namespace AuthMicroservice.Models;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid ProfileId { get; set; }
    
    public Profile Profile { get; set; }
}