using AutoMapper;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Application.Message;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System.Globalization;
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

        public async Task<APIResponse> GetTotalUser()
        {
            var totalUser = await _unitOfWork.UserRepository.GetTotalUsersAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.Complete,
                Data = new
                {
                    Total = totalUser
                }
            };
        }

        public async Task<APIResponse> GetByKeyWord(string keyWord)
        {
            var users = await _unitOfWork.UserRepository.GetUsersByKeywordAsync(keyWord);
            var usersResponse = _mapper.Map<IEnumerable<UserByKeywordResponseDto>>(users);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.Complete,
                Data = usersResponse
            };
        }

        public async Task<APIResponse> GetTotalUserByYear(int year)
        {
            var usersGroupedByMonth = await _unitOfWork.UserRepository.GetUsersCreatedInMonthAsync(year);

            var monthlyUserCounts = usersGroupedByMonth.Select(g => new
            {
                Month = g.Key,
                Total = g.Count()
            }).ToList();

            var allMonths = Enumerable.Range(1, 12).Select(i => new
            {
                Month = i,
                Total = 0
            }).ToList();

            var result = allMonths
                .GroupJoin(monthlyUserCounts,
                           m => m.Month,
                           u => u.Month,
                           (m, u) => new { Month = m.Month, Total = u.Sum(x => x.Total) })
                .Select(x => new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month),
                    Total = x.Total
                })
                .ToList();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.Complete,
                Data = result
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
                Message = Message.MessageCommon.Complete,
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
                    Message = MessageUser.UserNotFound,
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
                Message = Message.MessageCommon.UpdateSuccesfully,
                Data = existUsers,
            };
        }

        public async Task<APIResponse> DeleteUser(Guid userId)
        {
            var existUsers = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (existUsers == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageUser.UserNotFound,
                    Data = null,
                };
            }
            existUsers.Status = AccountStatus.Blocked.ToString();
            await _unitOfWork.UserRepository.Update(existUsers);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.DeleteSuccessfully,
                Data = null,
            };
        }

    }

}
