using AuthMicroservice.Models;
using AuthMicroservice.Models.Dtos;

namespace AuthMicroservice.Managers.Interfaces;

public interface IProfileManager
{
    Task<(string RefreshToken, string AcsesToken)> RegistrationAsync(RegistrationDto profile);
}