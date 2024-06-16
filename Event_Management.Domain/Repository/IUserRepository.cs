using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain.Repository
{
	public interface IUserRepository : IExtendRepository<User>
    {

        Task<User?> GetUser(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool?> IsExisted(UserFieldType field, string value);
        Task<bool> AddUser(User newUser);
        User? GetByEmail(string email);
        //Task<bool?> CheckRefreshTokenByUser(UserFieldType field, string value);
    }
}
