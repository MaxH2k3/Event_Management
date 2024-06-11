using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Application.Dto.UserDTO.Request;


namespace Event_Management.Application.Service
{
    public interface IUserService
    {
        Task<PagedList<User>> GetAllUser(int page, int eachPage);
        Task<User?> GetUser(Guid userId);
        Task<APIResponse> Register (RegisterUserDto registerUserDto);
        Task<APIResponse> Login(LoginUserDto loginUser);
        Task<APIResponse> Logout(string refreshToken);
        //Task<APIResponse> verifyAccount(string token, Guid id);
    }
}
