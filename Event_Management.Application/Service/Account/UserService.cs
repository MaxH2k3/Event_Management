using AutoMapper;
using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Application.ExternalServices;
using Event_Management.Domain;
using Event_Management.Domain.Constants;
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
        private readonly IAvatarApiClient _avatarApiClient;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper, IJWTService jWTService,
            IEmailService emailService, IConfiguration configuration,
            IAvatarApiClient avatarApiClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _JWTService = jWTService;
            _emailService = emailService;
            _configuration = configuration;
            _avatarApiClient = avatarApiClient;
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
                    StatusResponse = HttpStatusCode.Unauthorized,
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

            
            var userDTO = _mapper.Map<UserResponseDto>(user);
            switch (userDTO.Gender)
            {
                case UserGender.Male:
                    userDTO.Avatar = _avatarApiClient.GetRandomBoyAvatarUrl();
                    break;
                case UserGender.Female:
                    userDTO.Avatar = _avatarApiClient.GetRandomGirlAvatarUrl();
                    break;
                default:
                    userDTO.Avatar = _avatarApiClient.GetAvatarUrlWithName(userDTO.FirstName!, userDTO.LastName!);
                    break;
            };

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Login successfully",
                Data = new LoginResponseDto
                {
                    User = userDTO,
                    Token = new TokenResponseDTO
                    {
                        AccessToken = token,
                        RefreshToken = refreshTokenEntity.Token
                    }
                }
            };
        }


        public async Task<APIResponse> Register(RegisterUserDto newUser)
        {
            CreatePasswordHash(newUser.Password!, out byte[] passwordHash, out byte[] passwordSalt);
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
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
                //verifyToken = token,
                UpdatedAt = null,
            };
            bool isAdded = await _unitOfWork.UserRepository.AddUser(user);


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

        //public async Task<APIResponse> verifyAccount(string token, Guid id)
        //{
        //    var existUser = await _unitOfWork.UserRepository.GetUser(id);
        //    if (existUser == null)
        //    {
        //        return new APIResponse
        //        {
        //            StatusResponse = HttpStatusCode.NotFound,
        //            Message = MessageCommon.UserNotFound,
        //            Data = null
        //        };
        //    }
        //    else if (existUser.verifyToken != token)
        //    {
        //        return new APIResponse
        //        {
        //            StatusResponse = HttpStatusCode.NotFound,
        //            Message = "Invalid Token",
        //            Data = null
        //        };
        //    }
        //    existUser.Status = AccountStatus.Active.ToString();
        //    existUser.UpdatedAt = DateTime.UtcNow;
        //    existUser.verifyToken = null;
        //    await _unitOfWork.UserRepository.Update(existUser);
        //    await _unitOfWork.SaveChangesAsync();
        //    return new APIResponse
        //    {
        //        StatusResponse = HttpStatusCode.OK,
        //        Message = "Verify successfully",
        //        Data = null,
        //    };
        //}


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

        public async Task<PagedList<User>> GetAllUser(int page, int eachPage)
        {
            return await _unitOfWork.UserRepository.GetAll(page, eachPage, "Email");
        }
    }
}
