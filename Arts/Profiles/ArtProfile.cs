using Arts.Models.Dtos;
using AutoMapper;

namespace Arts.Profiles
{
    public class ArtProfile:Profile
    {
        public ArtProfile()
        {
           CreateMap<ArtDto,Art>().ReverseMap();
        }
    }
}
