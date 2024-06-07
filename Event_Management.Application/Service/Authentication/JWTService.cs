using Event_Management.Application.Dto.User;
using Event_Management.Domain;
using Event_Management.Domain.Constants.User;
using Event_Management.Domain.Models.JWT;
using Event_Management.Domain.Repository;
using Event_Management.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Security
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

        public async Task<string> GenerateAccessToken(LoginUserDto userDto)
        {

            var existUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email!);
            if (existUser == null)
            {
                return "Error! Unauthorized.";
            }


            List<Claim> claims = new List<Claim>
            {
                //currently fixing
                new Claim(UserClaimType.UserId, existUser.UserId.ToString()),
                new Claim(ClaimTypes.Email, existUser.Email!),
                new Claim(ClaimTypes.Role, existUser.Role.ToString()!)
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
                //currently fixing
                Expires = DateTime.Now.AddMinutes(2),
                Issuer = _configuration["JWTSetting:Issuer"],
                Audience = _configuration["JWTSetting:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(securityToken);

            return finaltoken;

        }


        public Task<string> GenerateRefreshToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}
