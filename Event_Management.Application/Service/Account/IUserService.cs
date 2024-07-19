using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Domain.Entity;


namespace Event_Management.Application.Service
{
    public interface IUserService
    {
        Task<APIResponse> GetTotalUser();
        Task<APIResponse> GetTotalUserByYear(int year);
        Task<APIResponse> GetByKeyWord(string keyWord);
        Task<APIResponse> GetAllUser(int page, int eachPage);
        Task<APIResponse> GetUserByIdAsync(Guid userId);
        User? GetUserById(Guid userId);
        Task<APIResponse> UpdateUser(UpdateDeleteUserDto updateUser);
        Task<APIResponse> DeleteUser(Guid id);


    }
}
