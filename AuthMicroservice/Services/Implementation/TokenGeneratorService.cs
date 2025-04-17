using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthMicroservice.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthMicroservice.Services.Implementation;

public class TokenGeneratorService(IConfiguration configuration, ILogger logger) : ITokenGeneratorService
{
    private readonly ILogger _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    public Task<string> GenerateTokenAsync(Guid userId, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var jwt = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AuthOptions:issuer"),
            audience: _configuration.GetValue<string>("AuthOptions:audience"),
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AuthOptions:key") ?? string.Empty)), SecurityAlgorithms.HmacSha256));
            
        _logger.LogDebug($"successful token generation for {userId}");
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
}