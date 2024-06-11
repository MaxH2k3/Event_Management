using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/Auth")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJWTService _jWTService;

        public AuthenticateController(IUserService userService, IJWTService jWTService)
        {
            _userService = userService;
            _jWTService = jWTService;
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
            return Ok(response);
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
            return Created(response.Message!, response.Data);
        }

        //[HttpPost("verify")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> VerifyAccount([FromQuery] string token, [FromQuery] Guid userId)
        //{
        //    var response = await _userService.verifyAccount(token, userId);
        //    if (response.StatusResponse != HttpStatusCode.OK)
        //    {
        //        return BadRequest(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var response = await _userService.Logout(request.RefreshToken!);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenResponseDTO token)
        {
            var response = await _jWTService.RefreshToken(token);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

    }
}
