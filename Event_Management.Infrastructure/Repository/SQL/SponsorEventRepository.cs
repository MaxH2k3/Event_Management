using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum.Sponsor;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Extensions;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class SponsorEventRepository : SQLExtendRepository<SponsorEvent>, ISponsorEventRepository
    {
        private readonly EventManagementContext _context;

        public SponsorEventRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsSponsored(Guid eventId)
        {
            var sponsorList = await _context.SponsorEvents.Where(s => s.EventId == eventId 
            && s.Status!.Equals(SponsorRequest.Confirmed.ToString())).ToListAsync();
            return sponsorList.Any();
        }
        public async Task<SponsorEvent?> CheckSponsoredEvent(Guid eventId, Guid userId)
        {
            return await _context.SponsorEvents
                         .FirstOrDefaultAsync(s => s.EventId == eventId
                                                && s.UserId == userId
                                                && s.Status!.Equals(SponsorRequest.Confirmed.ToString()));
        }

        public async Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId)
		{
			return await _context.SponsorEvents
						 .FirstOrDefaultAsync(s => s.EventId == eventId
												&& s.UserId == userId
												);
			
		}

        public async Task<SponsorEvent?> DeleteSponsorRequest(Guid eventId, Guid userId)
        {
            var sponsorRequest = await CheckSponsorEvent(eventId, userId);
            _context.SponsorEvents.Remove(sponsorRequest);
            await _context.SaveChangesAsync();
            return sponsorRequest;
        }

        public async Task<PagedList<SponsorEvent>> GetRequestSponsor(Guid userId, string? status, int page, int eachPage)
        {
            var list = _context.SponsorEvents.Where(s => s.UserId.Equals(userId));
            if(status != null)
            {
                list = list.Where(p => p.Status.Equals(status));
            }
            list = list.Include(p => p.Event);
            list = list.OrderByDescending(p => p.UpdatedAt);
            return await list.ToPagedListAsync(page, eachPage);
        }

        public async Task<PagedList<SponsorEvent>> GetSponsorEvents(SponsorEventFilter sponsorFilter)
        {
            var list = _context.SponsorEvents.Where(s => s.EventId.Equals(sponsorFilter.EventId)).OrderByDescending(p => p.CreatedAt).AsNoTracking().AsQueryable();

            if (sponsorFilter.Status != null)
            {
                list = list.Where(s => s.Status.Equals(sponsorFilter.Status));
            }
            
            if (sponsorFilter.IsSponsored.HasValue)
            {
                list = list.Where(s => s.IsSponsored == sponsorFilter.IsSponsored);
            }
            if(sponsorFilter.SponsorType != null)
            {
                list = list.Where(s => s.SponsorType.Equals(sponsorFilter.SponsorType));
            }
            list = list.Include(p => p.User);
            return await list.ToPagedListAsync(sponsorFilter.Page, sponsorFilter.EachPage);
        }
    }
}
