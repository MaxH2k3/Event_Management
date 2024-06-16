using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Infrastructure.Repository.SQL;

public class UserValidationRepository : SQLRepository<UserValidation>, IUserValidationRepository
{
    private readonly EventManagementContext _context;

    public UserValidationRepository(EventManagementContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserValidation?> GetUser(Guid userId)
    {
        return await _context.UserValidations.FirstOrDefaultAsync(x => x.UserId!.Equals(userId));
    }
}
