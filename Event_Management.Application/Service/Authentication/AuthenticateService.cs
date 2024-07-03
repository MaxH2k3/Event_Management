using AutoMapper;
using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Application.ExternalServices;
using Event_Management.Application.Helper;
using Event_Management.Application.Message;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Models.User;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Authentication
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _JWTService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAvatarApiClient _avatarApiClient;

        public AuthenticateService(IUnitOfWork unitOfWork , IMapper mapper, IJWTService jWTService, IEmailService emailService, IConfiguration configuration, IAvatarApiClient avatarApiClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _JWTService = jWTService;
            _emailService = emailService;
            _configuration = configuration;
            _avatarApiClient = avatarApiClient;
        }

        public async Task<APIResponse> SignInWithOTP(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "User not found",
                    Data = null
                };
            }

            return await GenerateAndSendOTP(user.Email!, user.FullName!, user.UserId);
        }
        public async Task<APIResponse> SignUpWithOTP(RegisterUserDto registerUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(registerUser.Email!);
            if (user != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User already exists",
                    Data = null
                };
            }

            var avatarApiClient = _avatarApiClient.GetAvatarUrlWithName(registerUser.FullName!);
            var id = Guid.NewGuid();
            user = new User
            {
                UserId = id,
                Email = registerUser.Email,
                FullName = registerUser.FullName,
                Status = AccountStatus.Pending.ToString(),
                Avatar = avatarApiClient,
                RoleId = Convert.ToInt32(UserRole.Guest),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            await _unitOfWork.UserRepository.Add(user);

            return await GenerateAndSendOTP(user.Email!, user.FullName!, user.UserId);

        }

        public async Task<APIResponse> SignInWithGoogle(LoginInWithGoogleDto googleUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(googleUser.Email);
            if (user == null)
            {
                return await CreateUserWithGoogleAccount(googleUser);

            };
            return await Login(googleUser);
        }

        private async Task<APIResponse> CreateUserWithGoogleAccount(LoginInWithGoogleDto googleUser)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Email = googleUser.Email,
                FullName = googleUser.FullName,
                Status = AccountStatus.Active.ToString(),
                Avatar = googleUser.PhotoUrl,
                RoleId = Convert.ToInt32(UserRole.Guest),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            await _unitOfWork.UserRepository.AddUser(newUser);

            var tokenResponse = await GenerateTokensForUser(newUser.UserId, newUser.Email);

            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(newUser);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.LoginSuccess,
                Data = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = tokenResponse
                }
            };
        }

        public async Task<APIResponse> Register(RegisterUserDto newUser)
        {
            // CreatePasswordHash(newUser.Password!, out byte[] passwordHash, out byte[] passwordSalt);
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            var id = Guid.NewGuid();
            var user = new User()
            {
                UserId = id,
                Email = newUser.Email,
                FullName = newUser.FullName,
                Status = AccountStatus.Active.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
            };
            bool isAdded = await _unitOfWork.UserRepository.AddUser(user);


            var result = await _emailService.SendEmailWithTemplate("Views/Template/VerifyAccount.cshtml", "Testing", new UserMailDto()
            {
                UserName = newUser.FullName!,
                UserId = id.ToString(),
                Email = user.Email!,
                Token = token
            });
            
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
        public async Task<APIResponse> Login(LoginInWithGoogleDto loginUser)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginUser.Email!);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageUser.UserNotFound,
                    Data = null
                };
            }

            //if (!VerifyPasswordHash(loginUser.Password!, user.Password!, user.PasswordSalt))
            //{
            //    return new APIResponse
            //    {
            //        StatusResponse = HttpStatusCode.Unauthorized,
            //        Message = MessageUser.LoginFailed,
            //        Data = null
            //    };
            //}

            //check if the refresh token exists, then remove it to create new refresh token ahihi
            var existingRefreshTokens = await _unitOfWork.RefreshTokenRepository.GetUserByIdAsync(user.UserId);
            if (existingRefreshTokens != null)
            {
                await _unitOfWork.RefreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshTokens.Token);
            }
            //create new refresh token
            var tokenResponse = await GenerateTokensForUser(user.UserId, user.Email!);
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageUser.LoginSuccess,
                Data = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = tokenResponse
                }
            };
        }
        public async Task<APIResponse> Logout(string userId)
        {

            var tokenEntity = await _unitOfWork.RefreshTokenRepository.GetUserByIdAsync(Guid.Parse(userId));
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
                Message = MessageUser.LogoutSuccess,
                Data = null
            };
        }
       
        public async Task<APIResponse> ValidateAccountWithToken(string token, Guid id)
        {
            var existUser = await _unitOfWork.UserValidationRepository.GetUser(id);
            if (existUser == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = null
                };
            }
            else if (existUser.VerifyToken != token)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "Invalid Token",
                    Data = null
                };
            }
            existUser.VerifyToken = null;
            await _unitOfWork.UserValidationRepository.Update(existUser);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Verify successfully",
                Data = null,
            };
        }
        public async Task<APIResponse> ValidateOTP(ValidateOtpDTO validateOtp)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(validateOtp.Email!);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = "User not found",
                    Data = null
                };
            }

            var existOTP = await _unitOfWork.UserValidationRepository.GetUser(user.UserId);
            if (existOTP!.Otp == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "OTP not found",
                    Data = null
                };
            }
            else if (existOTP!.ExpiredAt < DateTime.UtcNow)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "OTP Expired",
                    Data = null
                };
            }

            if (existOTP.Otp == validateOtp.Otp)
            {
                return await ProcessValidOTP(user, existOTP);
            }

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = "OTP validated failed",
                Data = null
            };
        }

        //Generate and sending email with OTP
        private async Task<APIResponse> GenerateAndSendOTP(string email, string userName, Guid userId)
        {
            var otp = AuthenHelper.GenerateOTP();
            var existingUserValidation = await _unitOfWork.UserValidationRepository.GetUser(userId);

            bool isNewUserValidation = existingUserValidation == null;
            var userValidation = existingUserValidation ?? new UserValidation { UserId = userId };

            userValidation.Otp = otp;
            userValidation.ExpiredAt = DateTime.UtcNow.AddMinutes(5);
            userValidation.CreatedAt = userValidation.CreatedAt == default ? DateTime.UtcNow : userValidation.CreatedAt;

            if (isNewUserValidation)
            {
                await _unitOfWork.UserValidationRepository.Add(userValidation);
            }
            else
            {
                await _unitOfWork.UserValidationRepository.Update(userValidation);
            }

            await _unitOfWork.SaveChangesAsync();

            var emailSent = await _emailService.SendEmailWithTemplate("Views/Template/VerifyWithOTP.cshtml", "Your OTP Code", new UserMailDto()
            {
                UserName = userName,
                Email = email,
                OTP = otp,
            });

           
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = "OTP sent successfully",
                    Data = null
                };
            
        }
        private async Task<APIResponse> ProcessValidOTP(User user, UserValidation userValidation)
        {
            userValidation.Otp = null;
            userValidation.ExpiredAt = null;


            var existingRefreshTokens = await _unitOfWork.RefreshTokenRepository.GetUserByIdAsync(user.UserId);
            if (existingRefreshTokens != null)
            {
                await _unitOfWork.RefreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshTokens.Token);
            }
            var tokenResponse = await GenerateTokensForUser(user.UserId, user.Email!);

            if (user.Status == AccountStatus.Pending.ToString())
            {
                user.Status = AccountStatus.Active.ToString();
            }

            await _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserValidationRepository.Update(userValidation);
            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OTP validated successfully",
                Data = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = tokenResponse
                }
            };
        }


        //áaaaaaaaaaaaaa
        private async Task<TokenResponseDTO> GenerateTokensForUser(Guid userId, string email)
        {
            var accessToken = await _JWTService.GenerateAccessToken(email);
            var refreshToken = _JWTService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMonths(Convert.ToInt32(_configuration["JWTSetting:RefreshTokenValidityInMonths"]))
            };

            await _unitOfWork.RefreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
