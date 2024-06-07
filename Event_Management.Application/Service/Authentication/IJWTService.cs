using Event_Management.Application.Dto.User;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Security
{
    public interface IJWTService
    {
        public Task<string> GenerateAccessToken(LoginUserDto user);
        public Task<string> GenerateRefreshToken(User user);
    }
}
