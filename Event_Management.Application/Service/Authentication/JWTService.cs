﻿using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Domain;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Event_Management.Application.Service
{
    public class JWTService : IJWTService
    {
        //private readonly JWTSetting _jwtSettings; //violence dependecy inversion principle
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public JWTService(/*JWTSetting jwtSetting,*/IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            //_jwtSettings = jwtSetting;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        //generate access token
        public async Task<string> GenerateAccessToken(LoginUserDto userDto)
        {

            var existUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email!);
            if (existUser == null)
            {
                return "Error! Unauthorized.";
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(UserClaimType.UserId, existUser.UserId.ToString()),
                new Claim(ClaimTypes.Email, existUser.Email!),
                new Claim(ClaimTypes.Role, existUser.Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWTSetting:Securitykey"] ?? throw new InvalidOperationException("Secret not configured")));

            var tokenhandler = new JwtSecurityTokenHandler();

            //var token = new JwtSecurityToken(
            //    issuer: _configuration["JWTSetting:Issuer"],
            //    audience: _configuration["JWTSetting:Audience"],
            //    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWTSetting:Issuer"])),
            //    claims: claims,
            //    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            //    );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JWTSetting:TokenExpiry"])),
                Issuer = _configuration["JWTSetting:Issuer"],
                Audience = _configuration["JWTSetting:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(securityToken);

            return finaltoken;

        }



        //Validate the token if the token is decoded with jwt, and then extract the information in the token
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                //ValidateAudience = true,
                //ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSetting:Securitykey"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }


        public async Task<APIResponse> RefreshToken(TokenResponseDTO token)
        {
            //validate the token if it's a jwt token or not, then extract information to create new token
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);

            var userIdClaim = principal!.Claims.FirstOrDefault(c => c.Type == UserClaimType.UserId);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (userIdClaim == null || emailClaim == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = "Invalid access token",
                    Data = null
                };
            }

            //check user existed in the refresh token
            var existUser = await _unitOfWork.RefreshTokenRepository.GetUserByIdAsync(Guid.Parse(userIdClaim!.Value));
            if (existUser == null)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = "Invalid token",
                    Data = null
                };
            }

            //check refresh token whether it's expired or null
            var existingRefreshToken = await _unitOfWork.RefreshTokenRepository.GetTokenAsync(token.RefreshToken!);
            if (existingRefreshToken == null || existingRefreshToken.ExpireAt <= DateTime.UtcNow)
            {
                return new APIResponse
                {
                    StatusResponse = System.Net.HttpStatusCode.Unauthorized,
                    Message = "Refresh token has expired",
                    Data = null
                };
            }

            //capture expired date from original token 
            var originalExpirationDate = existingRefreshToken.ExpireAt;

            // remove old refresh token
            await _unitOfWork.RefreshTokenRepository.RemoveRefreshTokenAsync(existingRefreshToken.Token);

            // generate new tokens
            var newAccessToken = await GenerateAccessToken(new LoginUserDto { Email = emailClaim.Value });
            var newRefreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = existUser.UserId,
                Token = newRefreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = originalExpirationDate
            };

            await _unitOfWork.RefreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = "Token refreshed successfully",
                Data = new TokenResponseDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };

            

        }

        //generate refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
