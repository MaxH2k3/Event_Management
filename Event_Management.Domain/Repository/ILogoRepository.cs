using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain.Repository
{
    public interface ILogoRepository : IExtendRepository<Logo>
    {
        Task<Logo> GetByName(string name);
    }
}
