using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service.Authentication
{
    public interface IAuthenticateService
    {
        Task<APIResponse> SignInWithOTP(string email);
        Task<APIResponse> SignUpWithOTP(RegisterUserDto registerUser);
        Task<APIResponse> SignInWithGoogle(LoginInWithGoogleDto loginInWithGoogleDto);
        Task<APIResponse> Login(LoginInWithGoogleDto loginUser);
        Task<APIResponse> Logout(string userId);
        Task<APIResponse> ValidateAccountWithToken(string token, Guid id);
        Task<APIResponse> ValidateOTP(ValidateOtpDTO validateOtp);
        //Task<APIResponse> Register(RegisterUserDto registerUserDto);
    }
}
