using Authentications.Models;
using Authentications.Models.AuthService.Models;
using Authentications.Models.Dtos;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Authentications.Profiles
{
   

    namespace AuthService.Profiles
    {
        public class AuthProfiles : Profile
        {
            public AuthProfiles()
            {
                CreateMap<RegisterUserDto, ApplicationUser>()
                    .ForMember(dest => dest.UserName, src => src.MapFrom(r => r.Email));

                CreateMap<UserDto, ApplicationUser>().ReverseMap();
            }
        }
    }

}
