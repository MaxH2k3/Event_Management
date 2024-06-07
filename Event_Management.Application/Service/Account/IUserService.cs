using Event_Management.Application.Dto.User;
using Event_Management.Domain;
using Event_Management.Domain.Models.System;


namespace Event_Management.API.Service
{
    public interface IUserService
    {
        Task<User?> GetUser(Guid userId);
        Task<APIResponse> Register (RegisterUserDto registerUserDto);
        Task<APIResponse> Login(LoginUserDto loginUser);
    }
}
