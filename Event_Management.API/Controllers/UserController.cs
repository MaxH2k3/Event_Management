using Event_Management.API.Service;
using Event_Management.Application.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            var response = await _userService.Login(loginUser);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return BadRequest(response);
            }
            return Ok(response.Data);
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUser)
        {
            var response = await _userService.Register(registerUser);
            if (response.StatusResponse != HttpStatusCode.Created)
            {
                return BadRequest(response);
            }
            return Created(response.Message, response.Data);
        }
    }
}
