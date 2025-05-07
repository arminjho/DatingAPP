using AutoMapper;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Extensions;

namespace DatingWebApp.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                 .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto,AppUser>();
            CreateMap<string,DateOnly>().ConvertUsing(s=>DateOnly.Parse(s));    
        }
    }
}
