using Grpc.Core;
using TinderAPI.Models.Entitys;
using TinderAPI.Repositories.Interfaces;

namespace TinderAPI.Services.Implementations;

public class ProfileGrpcService (IProfileRepository profileRepository) : CreateProfileService.CreateProfileServiceBase
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    public override async Task<CreateProfileResponse> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        await _profileRepository.AddAsync(new Profile
        {
            Id = Guid.Parse(request.Id),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            LastActive = DateTime.UtcNow,
            IsMale = request.IsMale,
            BirthdayDate = DateOnly.Parse(request.Birthday),
            Latitude = request.Latitude,
            Longitude = request.Longitude
        });
        Console.WriteLine("GRPC MESSAGE");
        return new CreateProfileResponse { Success = true, ProfileId = request.Id, ErrorMessage = String.Empty };
    }
}