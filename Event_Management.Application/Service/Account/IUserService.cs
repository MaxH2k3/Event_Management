using Event_Management.Domain.Models.User;
using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;


namespace Event_Management.Application.Service
{
    public interface IUserService
    {
        Task<PagedList<User>> GetAllUser(int page, int eachPage);
        Task<User?> GetUser(Guid userId);
        Task<APIResponse> Register (RegisterUserDto registerUserDto);
        Task<APIResponse> Login(LoginUserDto loginUser);
        Task<APIResponse> Logout(string refreshToken);
    }
}
