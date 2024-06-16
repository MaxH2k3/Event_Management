using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain;

public interface IUserValidationRepository : IRepository<UserValidation>
{
    Task<UserValidation?> GetUser(Guid userId);
}

