namespace AuthMicroservice.Services.Interfaces;

public interface ITokenGeneratorService
{
    Task<string> GenerateTokenAsync(Guid userId, DateTime expiresAt);
}