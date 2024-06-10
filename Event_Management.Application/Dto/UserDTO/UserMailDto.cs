using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.User
{
    public class UserMailDto
    {
        public string UserName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
