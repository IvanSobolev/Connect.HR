using AuthMicroservice.Models;
using AuthMicroservice.Models.Dtos;

namespace AuthMicroservice.Managers.Interfaces;

public interface IProfileManager
{
    Task<(string RefreshToken, string AccessToken)> RegistrationAsync(RegistrationDto profile);
}