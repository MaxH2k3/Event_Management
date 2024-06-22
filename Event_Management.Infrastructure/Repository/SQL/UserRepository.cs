using Event_Management.Domain;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Entity;

namespace Event_Management.Infrastructure.Repository.SQL
{
	public class UserRepository : SQLExtendRepository<User>, IUserRepository
    {
        private readonly EventManagementContext _context;

        public UserRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId!.Equals(userId));
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(a => a.Role).FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email.ToLower()));
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email!.ToLower().Equals(email.ToLower()));
        }


        public async Task<bool?> IsExisted(UserFieldType field, string value)
        {
            if (UserFieldType.UserId == field)
            {
                return (await GetAll()).Any(u => u.UserId!.Equals(value));
            }
            else if (UserFieldType.Email == field)
            {
                return (await GetAll()).Any(u => u.Email!.ToLower().Equals(value.ToLower()));
            }

            return false;
        }

        public async Task<bool> AddUser(User newUser)
        {
            if (_context.Users.Any(x => x.Email!.Equals(newUser.Email))){
                return false;
            }
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool?> CheckRefreshTokenByUser(UserFieldType field, string value)
        //{
        //   return await _context.Users
        //}
    }
}
