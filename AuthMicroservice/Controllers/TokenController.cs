using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers;

[ApiController]
[Route("auth/token")]
public class TokenController(IRefreshTokenManager refreshTokenManager) : ControllerBase
{
    private readonly IRefreshTokenManager _refreshTokenManager = refreshTokenManager;
    
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshTokensAsync([FromBody] string refreshToken)
    {
        TokensDto newTokens = await _refreshTokenManager.RefreshTokensAsync(refreshToken);

        if (newTokens.RefreshToken == String.Empty)
        {
            return BadRequest("Token is not active");
        }

        return Ok(newTokens);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> LogoutTokenAsync([FromBody] string refreshToken)
    {
        _ = _refreshTokenManager.LogoutTokenAsync(refreshToken);
        return Ok("Token logout");
    }

    [HttpPost]
    [Route("logoutall")]
    public async Task<IActionResult> LogoutAllTokenAsync([FromBody] string refreshToken)
    {
        _ = _refreshTokenManager.LogoutAllTokenAsync(refreshToken);
        return Ok("All token logout");
    }
}