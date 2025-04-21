using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthMicroservice.Services.Implementation;

public class TokenGeneratorService(AuthOptions authOptions, ILogger<TokenGeneratorService> logger) : ITokenGeneratorService
{
    private readonly ILogger<TokenGeneratorService> _logger = logger;
    private readonly AuthOptions _configuration = authOptions;
    public Task<string> GenerateTokenAsync(Guid userId, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var jwt = new JwtSecurityToken(
                issuer: authOptions.Issuer,
                audience: authOptions.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)), SecurityAlgorithms.HmacSha256)
            );
            
        _logger.LogDebug($"successful token generation for {userId}");
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
}