using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Repositories.Interfaces;
using AuthMicroservice.Services.Interfaces;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Identity;
using TinderAPI;

namespace AuthMicroservice.Managers.Implementations;

public class ProfileManager ( IProfileRepository profileRepository, IRefreshTokenRepository refreshTokenRepository, ITokenGeneratorService tokenGenerator, ILogger logger) : IProfileManager
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly ITokenGeneratorService _tokenGenerator = tokenGenerator;
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
    private readonly ILogger _logger = logger;
    public async Task<(string RefreshToken, string AccessToken)> RegistrationAsync(RegistrationDto profile)
    {
        if (await _profileRepository.ExistsByEmailAsync(profile.Email))
        {
            _logger.LogWarning("Registration attempt with existing email: {Email}", profile.Email);
            return new ValueTuple<string, string>("", "");
        }

        Guid userId = Guid.NewGuid();

        string passwordHash = _passwordHasher.HashPassword(userId.ToString(), profile.Password);
        
        using var channel = GrpcChannel.ForAddress("http://localhost:5001");
        var client = new CreateProfileService.CreateProfileServiceClient(channel);

        var reply = await client.CreateProfileAsync(new CreateProfileRequest
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
            _logger.LogError(reply.ErrorMessage);
            return new ValueTuple<string, string>("", "");
        }

        string refreshToken = await _tokenGenerator.GenerateTokenAsync(userId, DateTime.UtcNow.AddDays(30));
        string accessToken = await _tokenGenerator.GenerateTokenAsync(userId, DateTime.UtcNow.AddMinutes(15));
        
        await _profileRepository.AddAsync(userId, profile.Email, passwordHash);
        await _refreshTokenRepository.AddAsync(refreshToken, DateTime.UtcNow.AddDays(30), userId);
        
        return new ValueTuple<string, string>(refreshToken, accessToken);
    }
}