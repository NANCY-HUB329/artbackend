using OrderService.Models.Dtos;

namespace OrderService.Services.IServices
{
    public interface IUser
    {
        Task<UserDto> GetUserById(string Id);
    }
}
