using Event_Management.Domain.Entity;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;

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
	}
}
