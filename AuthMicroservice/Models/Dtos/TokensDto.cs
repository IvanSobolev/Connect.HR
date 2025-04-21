namespace AuthMicroservice.Models.Dtos;

public class TokensDto (string? accessToken = null, string? refreshToken = null)
{
    public string AccessToken { get; set; } = accessToken ?? String.Empty;
    public string RefreshToken { get; set; } = refreshToken ?? String.Empty;
}