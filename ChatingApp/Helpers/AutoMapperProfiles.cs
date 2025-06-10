using AutoMapper;
using ChatingApp.Dtos;
using ChatingApp.Extentions;
using ChatingApp.Models;

namespace ChatingApp.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(d=>d.Age,o=>o.MapFrom(s=>s.DateOfBirth.CalculateAge()))
                .ForMember(d => d.PhotoUrl, o => 
                o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
