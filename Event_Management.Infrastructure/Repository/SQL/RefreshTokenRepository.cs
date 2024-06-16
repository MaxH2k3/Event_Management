using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class RefreshTokenRepository : SQLExtendRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly EventManagementContext _context;

        public RefreshTokenRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetUserByIdAsync(Guid id)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<RefreshToken?> GetTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<bool> AddRefreshToken(RefreshToken newRefreshToken)
        {
            if (_context.RefreshTokens.Any(x => x.Token.Equals(newRefreshToken.Token)))
            {
                return false;
            }
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            return true;
        }

        public async Task<bool> RemoveRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken == null)
            {
                return false; 
            }

            _context.RefreshTokens.Remove(refreshToken);
            //await _context.SaveChangesAsync();

            return true;
        }

    }
}
