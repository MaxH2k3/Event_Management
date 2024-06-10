using Event_Management.Domain.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Repository
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetTokenAsync(string refreshToken);
        Task<RefreshToken?> GetUserByIdAsync(Guid id);
        Task<bool> AddRefreshToken(RefreshToken newRefreshToken);
        Task<bool> RemoveRefreshTokenAsync(string token);
    }
}
