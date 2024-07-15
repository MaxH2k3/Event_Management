using AutoMapper;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System.Net;

namespace Event_Management.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetByKeyWord(string keyWord)
        {
            var users = await _unitOfWork.UserRepository.GetUsersByKeywordAsync(keyWord);
            var usersResponse = _mapper.Map<IEnumerable<UserByKeywordResponseDto>>(users);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.GetSuccesfully,
                Data = usersResponse
            };
        }

        public User? GetUserById(Guid userId)
        {
            return _unitOfWork.UserRepository.GetUserById(userId);
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
        }

        public async Task<APIResponse> GetAllUser(int page, int eachPage)
        {
            IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllUser(page, eachPage, "Email");
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Ok",
                Data = _mapper.Map<IEnumerable<UserResponseDto>>(users),
            };
        }

        public async Task<APIResponse> UpdateUser(UpdateDeleteUserDto updateUser)
        {
            var existUsers = await _unitOfWork.UserRepository.GetUserByEmailAsync(updateUser.Email!);
            if (existUsers == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "User not existed",
                    Data = null,
                };
            }
            existUsers.FullName = updateUser.FullName;
            existUsers.Phone = updateUser.Phone;
            existUsers.Avatar = updateUser.Avatar;
            await _unitOfWork.UserRepository.Update(existUsers);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Update successfully",
                Data = existUsers,
            };
        }

        public async Task<APIResponse> DeleteUser(UpdateDeleteUserDto deleteUser)
        {
            var existUsers = await _unitOfWork.UserRepository.GetUserByEmailAsync(deleteUser.Email!);
            if (existUsers == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "User not existed",
                    Data = null,
                };
            }
            existUsers.Status = AccountStatus.Blocked.ToString();
            await _unitOfWork.UserRepository.Update(existUsers);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Delete successfully",
                Data = null,
            };
        }

    }

}
