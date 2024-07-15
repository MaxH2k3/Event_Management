﻿using Event_Management.Domain.Entity;
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

		public async Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId)
		{
			return await _context.SponsorEvents
						 .FirstOrDefaultAsync(s => s.EventId == eventId
												&& s.UserId == userId
												&& s.Status == "CONFIRMED");
			
		}

        public async Task<PagedList<SponsorEvent>> GetSponsoredEvent(Guid userId, int page, int eachPage)
        {
            var list = _context.SponsorEvents.Where(s => s.UserId.Equals(userId)).OrderByDescending(p => p.UpdatedAt);
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
            list = list.Include(p => p.User);
            return await list.ToPagedListAsync(sponsorFilter.Page, sponsorFilter.EachPage);
        }
    }
}
