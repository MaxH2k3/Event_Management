using AutoMapper;
using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Domain;
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
        private readonly IMapper _mapper;
        private readonly IJWTService _JWTService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService, IEmailService emailService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _JWTService = jWTService;
            _emailService = emailService;
            _configuration = configuration;
        }

       

        public Task<User?> GetUser(Guid userId)
        {
            throw new NotImplementedException();
        }

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

            //check if the refresh token exists, then remove it to create new refresh token ahihi
            var existingRefreshTokens = await _unitOfWork.RefreshTokenRepository.GetUserByIdAsync(user.UserId);
            if (existingRefreshTokens != null)
            {
                await _unitOfWork.RefreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshTokens.Token);
            }

            //create new refresh token
            var token = await _JWTService.GenerateAccessToken(loginUser);
            var refresh = _JWTService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMonths(Convert.ToInt32(_configuration["JWTSetting:RefreshTokenValidityInMonths"]))
            };


            await _unitOfWork.RefreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Login successfully",
                Data = new TokenResponseDTO
                {
                    AccessToken = token,
                    RefreshToken = refreshTokenEntity.Token
                }
            };
        }


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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
            };
            bool isAdded = await _unitOfWork.UserRepository.AddUser(user);
            ////send mail
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            var result = await _emailService.SendEmailWithTemplate("Views/Template/VerifyAccount.cshtml", "Testing", new UserMailDto()
            {
                UserName = user.FirstName + " " + user.LastName,
                UserId = id.ToString(),
                Email = user.Email!,
                Token = token
            });

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

        //logout account
        public async Task<APIResponse> Logout(string refreshToken)
        {
            var tokenEntity = await _unitOfWork.RefreshTokenRepository.GetTokenAsync(refreshToken);
            if (tokenEntity == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "Invalid refresh token",
                    Data = null
                };
            }

            await _unitOfWork.RefreshTokenRepository.RemoveRefreshTokenAsync(tokenEntity.Token);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Logged out successfully",
                Data = null
            };
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

        public Task<PagedList<User>> GetAllUser(int page, int eachPage)
        {
            throw new NotImplementedException();
        }
    }
}
