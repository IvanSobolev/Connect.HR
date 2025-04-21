using Grpc.Core;

namespace TinderAPI.Services.Implementations;

public class UserGrpcService : CreateProfileService.CreateProfileServiceBase
{
    public async override Task<CreateProfileResponse> CreateProfile(CreateProfileRequest request, ServerCallContext context)
    {
        Console.WriteLine(request.FirstName);
        return new CreateProfileResponse { Success = true, ProfileId = request.Id, ErrorMessage = String.Empty };
    }
}