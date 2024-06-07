using AutoMapper;
using Event_Management.API.Service;
using Event_Management.Application.Dto.User;
using Event_Management.Application.Service.Email;
using Event_Management.Application.Service.Security;
using Event_Management.Domain;
using Event_Management.Domain.Enum.User;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Account
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService JWTService;
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            JWTService = jWTService;
            _emailService = emailService;
        }

        public Task<User?> GetUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        //still working on login
        public async Task<APIResponse> Login(LoginUserDto loginUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginUser.Email!);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User not existed",
                    Data = null
                };
            }

            if (!VerifyPasswordHash(loginUser.Password!, user.Password!, user.PasswordSalt))
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "Wrong password",
                    Data = null
                };
            }

            var token = await JWTService.GenerateAccessToken(loginUser);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Login successfully",
                Data = token
            };

        }

        // still working on register
        public async Task<APIResponse> Register(RegisterUserDto newUser)
        {
            CreatePasswordHash(newUser.Password!, out byte[] passwordHash, out byte[] passwordSalt);
            var id = Guid.NewGuid();
            var user = new User()
            {
                UserId = id,
                Email = newUser.Email,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Age = newUser.Age,
                Gender = newUser.Gender,
                Phone = newUser.Phone,
                RoleId = newUser.RoleId,
                Status = AccountStatus.Pending.ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
            };
            bool isAdded = await _unitOfWork.UserRepository.AddUser(user);
            ////send mail
            //string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            //var result = await _emailService.SendEmailWithTemplate("Template/Index.cshtml", "Testing", new UserMailDto()
            //{
            //    UserName = user.FirstName + user.LastName,
            //    UserId = id.ToString(),
            //    Email = user.Email!,
            //    Token = token
            //});

            //validation whether user data insert successfully in db
            if (isAdded)
            {
                await _unitOfWork.SaveChangesAsync();
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.Created,
                    Message = "User registered successfully",
                    Data = user
                };
            }
            else
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "Failed to register user",
                    Data = null
                };
            }

        }


        //Still confused about where I should put this code in which layers?????????????????????????????????????
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }
    }
}
