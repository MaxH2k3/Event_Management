

using Event_Management.Application.Dto.AuthenticationDTO;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Models.User;
using Event_Management.Domain.UnitOfWork;
using System.Net;
using System.Security.Cryptography;

namespace Event_Management.Application.Helper
{
    public class AuthenHelper
    {
        public static string GenerateOTP()
        {
            var randomNumber = new byte[6];
            RandomNumberGenerator.Fill(randomNumber);
            var otp = BitConverter.ToUInt32(randomNumber, 0) % 1000000;
            return otp.ToString("D6");
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }
        
    }
}
