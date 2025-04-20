using AuthMicroservice.Managers.Implementations;
using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers;

[Route("auth/profile")]
public class ProfileController (IProfileManager profileManager) : ControllerBase
{
    private readonly IProfileManager _profileManager = profileManager;

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationDto profile)
    {
        TokensDto tokens = await _profileManager.RegistrationAsync(profile);
        if (tokens.RefreshToken == String.Empty)
        {
            return BadRequest(tokens.AccessToken);
        }

        return Ok(tokens);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto profile)
    {
        TokensDto tokens = await _profileManager.LoginAsync(profile);
        if (tokens.RefreshToken == String.Empty)
        {
            return BadRequest("Incorrect email or password");
        }

        return Ok(tokens);
    }
}