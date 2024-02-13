using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Authentications.Models.Dtos;
using Authentications.Services.IServices;
using AutoMapper;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Authentications.Services.IServices.AuthService.Services.IServices;

namespace Authentications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly ResponseDto _response;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUser user, IConfiguration configuration, IMapper mapper)
        {
            _userService = user;
            _configuration = configuration;
            _response = new ResponseDto();
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var res = await _userService.RegisterUser(registerUserDto);

            if (string.IsNullOrWhiteSpace(res))
            {
                _response.Result = "User Registered Successfully";

                // Determine the role based on registration information
                string role = registerUserDto.Role;

                // Assign the determined role to the user
                await _userService.AssignUserRoles(registerUserDto.Email, role);

                var message = new UserMessageDto()
                {
                    Name = registerUserDto.Name,
                    Email = registerUserDto.Email,
                };

                await _userService.PublishMessageToServiceBus(message, _configuration.GetValue<string>("ServiceBus:register"));

                return Created("", _response);
            }

            _response.Errormessage = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> loginUser(LoginRequestDto loginRequestDto)
        {
            var res = await _userService.loginUser(loginRequestDto);

            if (res.User != null)
            {

                _response.Result = res;
                return Created("", _response);
            }


            _response.Errormessage = "Invalid Credentials";
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        [HttpPost("assignrole")]
        public async Task<ActionResult<ResponseDto>> assignrole(AssignRoleDto role)
        {
            var res = await _userService.AssignUserRoles(role.Email, role.Role);

            if (res != null)
            {

                _response.Result = res;
                return Ok(_response);
            }


            _response.Errormessage = "error occurred";


            return (_response);

        }

            [HttpGet("{Id}")]
            public async Task<ActionResult<ResponseDto>> GetUser(string Id)
            {
                var res = await _userService.GetUserById(Id);
                var user = _mapper.Map<UserDto>(res);
                if (res != null)
                {

                    _response.Result = user;
                    return Ok(_response);
                }


                _response.Errormessage = "User Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }

           
        }
    }


