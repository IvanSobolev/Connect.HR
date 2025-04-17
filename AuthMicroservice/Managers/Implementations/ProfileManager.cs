using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Repositories.Interfaces;
using AuthMicroservice.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TinderAPI;

namespace AuthMicroservice.Managers.Implementations;

public class ProfileManager ( IProfileRepository profileRepository, IRefreshTokenRepository refreshTokenRepository, 
    ITokenGeneratorService tokenGenerator, IProfileMicroserviceClient profileMicroserviceClient, ILogger logger) : IProfileManager
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly ITokenGeneratorService _tokenGenerator = tokenGenerator;
    private readonly IProfileMicroserviceClient _profileMicroserviceClient = profileMicroserviceClient;
    private readonly ILogger _logger = logger;
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
    public async Task<TokensDto> RegistrationAsync(RegistrationDto profile)
    {   
        if (await _profileRepository.ExistsByEmailAsync(profile.Email))
        {
            _logger.LogWarning("Registration attempt with existing email: {Email}", profile.Email);
            return new TokensDto();
        }

        Guid userId = Guid.NewGuid();

        string passwordHash = _passwordHasher.HashPassword(userId.ToString(), profile.Password);
        
        var reply = await _profileMicroserviceClient.CreateProfileAsync(new CreateProfileRequest
        {
            Id = userId.ToString(),
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            IsMale = profile.IsMale,
            Birthday = profile.BirthdayDate.ToString(),
            Latitude = profile.Latitude,
            Longitude = profile.Longitude
        });

        if (!reply.Success)
        {
            _logger.LogError("Profile service error: " +  reply.ErrorMessage);
            return new TokensDto();
        }

        string refreshToken = await _tokenGenerator.GenerateTokenAsync(userId, DateTime.UtcNow.AddDays(30));
        string accessToken = await _tokenGenerator.GenerateTokenAsync(userId, DateTime.UtcNow.AddMinutes(15));
        
        await _profileRepository.AddAsync(userId, profile.Email, passwordHash);
        await _refreshTokenRepository.AddAsync(refreshToken, DateTime.UtcNow.AddDays(30), userId);
        
        _logger.LogDebug("Sucess attempt registration with email: {Email}", profile.Email);
        return new TokensDto(accessToken, refreshToken);
    }

    public async Task<TokensDto> LoginAsync(LoginDto profile)
    {
        var dbUser = await _profileRepository.GetByEmailAsync(profile.Email);
        if (dbUser == null)
        {
            _logger.LogWarning("Login attempt with not existing email: {Email}", profile.Email);
            return new TokensDto();
        }
        string passwordHash = _passwordHasher.HashPassword(dbUser.Id.ToString(), profile.Password);
        if (dbUser.PasswordHash != passwordHash)
        {
            _logger.LogWarning("Login attempt with wrong password to email: {Email}", profile.Email);
            return new TokensDto();
        }
        
        string refreshToken = await _tokenGenerator.GenerateTokenAsync(dbUser.Id, DateTime.UtcNow.AddDays(30));
        string accessToken = await _tokenGenerator.GenerateTokenAsync(dbUser.Id, DateTime.UtcNow.AddMinutes(15));
        
        await _refreshTokenRepository.AddAsync(refreshToken, DateTime.UtcNow.AddDays(30), dbUser.Id);
        _logger.LogDebug("successful attempt to login an account with mail: {Email}", profile.Email);
        return new TokensDto(accessToken, refreshToken);
    }
}