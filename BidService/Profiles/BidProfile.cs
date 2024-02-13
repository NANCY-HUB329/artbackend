using AutoMapper;
using BidService.Models;
using BidService.Models.Dtos;

namespace BidService.Profiles
{
    public class BidProfile:Profile
    {
        public BidProfile()
        {
            CreateMap<AddBidDto,Bid>().ReverseMap();
        }
    }
}
