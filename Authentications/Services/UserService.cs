using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentications.Data;
using Authentications.Models.AuthService.Models;
using Authentications.Models.Dtos;
using Authentications.Services.Iservices.AuthService.Services.IServices;
using Authentications.Services.IServices.AuthService.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Authentications.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwt _JwtServices;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwt jwtService, IConfiguration configuration)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _JwtServices = jwtService;
            _configuration = configuration;
            
        }

        public async Task<bool> AssignUserRoles(string Email, string RoleName)
        {
            var user = await _context.ApplicationUsers
                .Where(x => x.Email.ToLower() == Email.ToLower())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }
            else
            {
                if (!await _roleManager.RoleExistsAsync(RoleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(RoleName));
                }

                await _userManager.AddToRoleAsync(user, RoleName);
                return true;
            }
        }

        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await _context.ApplicationUsers
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<LoginResponseDto> loginUser(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUsers
                .Where(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower())
                .FirstOrDefaultAsync();

            if (user == null || !_userManager.CheckPasswordAsync(user, loginRequestDto.Password).GetAwaiter().GetResult())
            {
                return new LoginResponseDto();
            }

            var loggeduser = _mapper.Map<UserDto>(user);

            var roles = await _userManager.GetRolesAsync(user);

            var token = _JwtServices.GenerateToken(user, roles);

            var response = new LoginResponseDto()
            {
                User = loggeduser,
                Token = token
            };

            return response;
        }

        public async Task<string> RegisterUser(RegisterUserDto userDto)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(userDto);

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(userDto.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(userDto.Role));
                    }

                    await _userManager.AddToRoleAsync(user, userDto.Role);
                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task PublishMessageToServiceBus(UserMessageDto message, string queueName)
        {
            try
            {
                string connectionString = _configuration.GetSection("ConnectionStrings:AzureServiceBusConnection").Value;
                string queueNameNow = _configuration.GetSection("ServiceBus:register").Value;


                var serviceBusConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString");
                Console.WriteLine("hello world");

                var client = new QueueClient(connectionString, queueNameNow);

                var messageBody = JsonConvert.SerializeObject(message);
                var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(messageBody));

                await client.SendAsync(serviceBusMessage);
                Console.WriteLine("hello nancy");


                await client.CloseAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error publishing message to Service Bus: {ex.Message}");
            }
        }
    }
}
