using AuthMicroservice.Managers.Interfaces;
using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Repositories.Interfaces;
using Grpc.Net.Client;
using TinderAPI;

namespace AuthMicroservice.Managers.Implementations;

public class ProfileManager ( IProfileRepository profileRepository ) : IProfileManager
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    public async Task<(string RefreshToken, string AcsesToken)> RegistrationAsync(RegistrationDto profile)
    {
        if (await _profileRepository.ExistsByEmailAsync(profile.Email))
        {
            return new ValueTuple<string, string>("", "");
        }
        
        using var channel = GrpcChannel.ForAddress("http://localhost:5001");
        var client = new CreateProfileService.CreateProfileServiceClient(channel);

        //var reply = await client.CreateProfile(new CreateProfileRequest { });
    }
}