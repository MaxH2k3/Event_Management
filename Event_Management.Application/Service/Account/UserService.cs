using AutoMapper;
using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Application.ExternalServices;
using Event_Management.Application.Message;
using Event_Management.Domain;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Models.User;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;

namespace Event_Management.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Task<User?> GetUser(Guid userId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<PagedList<User>> GetAllUser(int page, int eachPage)
        {
            return await _unitOfWork.UserRepository.GetAll(page, eachPage, "Email");
        }

        
    }

}
