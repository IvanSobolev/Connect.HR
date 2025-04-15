using TinderAPI.Models.Entitys;
using TinderAPI.Models.DTOs;
using Profile = TinderAPI.Models.Entitys.Profile;

namespace TinderAPI.Helpers;

public class AutoMapperProfiles : AutoMapper.Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Profile, ProfileDto>()
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => CalculateAge(src.BirthdayDate)))
            .ForMember(dest => dest.Photos,
                opt => opt.MapFrom(src => src.Photos.OrderBy(p => p.SortOrder)));

        CreateMap<Photo, PhotoDto>();
        CreateMap<Hobby, HobbyDto>();
        CreateMap<Preferences, PreferencesDto>();
        CreateMap<CreateProfileDto, Profile>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
    
    private int CalculateAge(DateOnly birthday)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthday.Year;
        if (birthday > today.AddYears(-age)) age--;
        return age;
    }
}