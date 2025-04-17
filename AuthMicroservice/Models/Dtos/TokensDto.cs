namespace AuthMicroservice.Models.Dtos;

public class TokensDto (string accessToken = null, string refreshToken = null)
{
    public string? AccessToken { get; set; } = accessToken;
    public string? RefreshToken { get; set; } = refreshToken;
}