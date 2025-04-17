using TinderAPI;

namespace AuthMicroservice.Services.Interfaces;

public interface IProfileMicroserviceClient
{
    Task<CreateProfileResponse> CreateProfileAsync(CreateProfileRequest profileRequest);
}