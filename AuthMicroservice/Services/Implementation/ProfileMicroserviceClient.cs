using AuthMicroservice.Models.Dtos;
using AuthMicroservice.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using TinderAPI;

namespace AuthMicroservice.Services.Implementation;

public class ProfileMicroserviceClient : IProfileMicroserviceClient
{
    private readonly ILogger<ProfileMicroserviceClient> _logger;
    private readonly GrpcChannel _channel;
    private readonly CreateProfileService.CreateProfileServiceClient _client;
    
    public ProfileMicroserviceClient(
        GrpcOptions grpcOptions,
        ILogger<ProfileMicroserviceClient> logger)
    {
        _logger = logger;
        
        var grpcAddress = grpcOptions.Address;
        _channel = GrpcChannel.ForAddress(grpcAddress, new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                EnableMultipleHttp2Connections = true
            }
        });
        
        _client = new CreateProfileService.CreateProfileServiceClient(_channel);
    }
    
    public async Task<CreateProfileResponse> CreateProfileAsync(CreateProfileRequest profileRequest)
    {
        try
        {
            var deadline = DateTime.UtcNow.AddSeconds(5);
            var response = await _client.CreateProfileAsync(
                profileRequest,
                deadline: deadline);
            
            _logger.LogDebug("Successful profile submission");
            return response;
        }
        catch(RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            _logger.LogError(ex, "CreateProfile deadline exceeded for user {UserId}", profileRequest.Id);
            return new CreateProfileResponse
            {
                Success = false, 
                ProfileId = null, 
                ErrorMessage = "CreateProfile deadline exceeded for user {UserId}"
            };
        }
    }
}