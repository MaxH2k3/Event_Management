using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain.Repository
{
    public interface ISponsorEventRepository : IExtendRepository<SponsorEvent>
    {
        Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId);
    }
}
