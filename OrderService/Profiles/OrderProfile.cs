using AutoMapper;
using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Profiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<MakeOrderDto, Orders>().ReverseMap();
        }
    }
}
