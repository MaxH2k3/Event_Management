using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain.Repository
{
    public interface ISponsorEventRepository : IExtendRepository<SponsorEvent>
    {
        Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId);
        Task<PagedList<SponsorEvent>> GetSponsorEvents(SponsorEventFilter sponsorFilter);
        Task<PagedList<SponsorEvent>> GetRequestSponsor(Guid userId, string? status,  int page, int eachPage);
        Task<SponsorEvent?> DeleteSponsorRequest(Guid eventId, Guid userId);

    }
}
