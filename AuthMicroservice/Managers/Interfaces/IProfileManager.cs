using AuthMicroservice.Models;
using AuthMicroservice.Models.Dtos;

namespace AuthMicroservice.Managers.Interfaces;

public interface IProfileManager
{
    Task<TokensDto> RegistrationAsync(RegistrationDto profile);
    Task<TokensDto> LoginAsync(LoginDto profile);
}