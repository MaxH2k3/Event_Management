using Event_Management.Application;
using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Service;
using Event_Management.Application.Service.Authentication;
using Event_Management.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Event_Management.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IAuthenticateService _authenService;
        private readonly IJWTService _jWTService;

        public AuthenticateController(IAuthenticateService authenticateService, IJWTService jWTService)
        {
            _authenService = authenticateService;
            _jWTService = jWTService;
        }


        // [HttpPost("login")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        // {
        //     var response = await _authenService.Login(loginUser);
        //     if (response.StatusResponse != HttpStatusCode.OK)
        //     {
        //         return BadRequest(response);
        //     }
        //     return Ok(response);
        // }

        // [HttpPost("SignInWithGoogle")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> SignInWithGoogle([FromBody] LoginUserDto loginUser)
        // {
        //     var response = await _authenService.Login(loginUser);
        //     if (response.StatusResponse != HttpStatusCode.OK)
        //     {
        //         return BadRequest(response);
        //     }
        //     return Ok(response);
        // }

        [HttpPost("sign-in/google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInWithGoogle([FromBody] LoginInWithGoogleDto loginInWithGoogle)
        {
            var response = await _authenService.SignInWithGoogle(loginInWithGoogle);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("sign-in/otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInWithOTP([FromBody] LoginUserDto loginUser)
        {
            var response = await _authenService.SignInWithOTP(loginUser.Email!);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPost("sign-up/otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUpWithOTP([FromBody] RegisterUserDto registerUser)
        {
            var response = await _authenService.SignUpWithOTP(registerUser);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("otp/validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateOTP([FromBody] ValidateOtpDTO validateOtp)
        {
            var response = await _authenService.ValidateOTP(validateOtp);
            if (response.StatusResponse != HttpStatusCode.OK)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        // [HttpPost("register")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUser)
        // {
        //     var response = await _authenService.Register(registerUser);
        //     if (response.StatusResponse != HttpStatusCode.Created)
        //     {
        //         return BadRequest(response);
        //     }
        //     return Created(response.Message!, response.Data);
        // }

        // [HttpPost("verify")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> VerifyAccount([FromQuery] string token, [FromQuery] Guid userId)
        // {
        //    var response = await _authenService.ValidateAccountWithToken(token, userId);
        //    if (response.StatusResponse != HttpStatusCode.OK)
        //    {
        //        return BadRequest(response);
        //    }
        //    return Ok(response);
        // }


        [Authorize]
        [HttpPost("sign-out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.GetUserIdFromToken();
            var response = await _authenService.Logout(userId);
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
