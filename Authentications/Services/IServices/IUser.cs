using Authentications.Models.AuthService.Models;
using Authentications.Models.Dtos;


namespace Authentications.Services.IServices
{
    namespace AuthService.Services.IServices
    {
        public interface IUser
        {

            Task<string> RegisterUser(RegisterUserDto userDto);

            Task<LoginResponseDto> loginUser(LoginRequestDto loginRequestDto);

            Task<bool> AssignUserRoles(string Email, string RoleName);

            Task<ApplicationUser> GetUserById(string Id);

            Task PublishMessageToServiceBus(UserMessageDto message, string queueName);


        }
    }

}
