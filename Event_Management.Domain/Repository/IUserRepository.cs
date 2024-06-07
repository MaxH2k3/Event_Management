using Event_Management.Domain.Enum.User;
using Event_Management.Domain.Repository.Common;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Repository
{
    public interface IUserRepository : IExtendRepository<User>
    {

        Task<User?> GetUser(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool?> IsExisted(UserFieldType field, string value);
        Task<bool> AddUser(User newUser);
        User? GetByEmail(string email);
    }
}
