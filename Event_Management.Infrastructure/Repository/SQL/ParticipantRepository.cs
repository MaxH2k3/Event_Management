using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Extensions;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Event_Management.Domain.Entity;

namespace Event_Management.Infrastructure.Repository.SQL
{
	public class ParticipantRepository : SQLExtendRepository<Participant>, IParticipantRepository
    {
        private readonly EventManagementContext _context;
		private readonly ICacheRepository _cacheRepository;

		public ParticipantRepository(EventManagementContext context, ICacheRepository cacheRepository) : base(context)
        {
            _context = context;
			_cacheRepository = cacheRepository;
		}

		public async Task<bool> IsExistedOnEvent(Guid userId, Guid eventId)
		{
			return await _context.Participants.AnyAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
		}

		public async Task<Participant?> GetParticipant(Guid userId, Guid eventId)
		{
			return await _context.Participants.FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
		}

		public async Task<PagedList<Participant>> FilterDataParticipant(FilterParticipant filter)
		{
			var query = _context.Participants.AsNoTracking().AsQueryable();

			if (filter.EventId.HasValue)
			{
				query = query.Where(p => p.EventId.Equals(filter.EventId));
			}

			if (filter.RoleEventId.HasValue)
			{
				query = query.Where(p => p.RoleEventId == filter.RoleEventId);
			}

			if (filter.CheckedIn.HasValue)
			{
				query = query.Where(p => p.CheckedIn <= filter.CheckedIn);
			}

			if (filter.IsCheckedMail.HasValue)
			{
				query = query.Where(p => p.IsCheckedMail == filter.IsCheckedMail);
			}

			if (filter.CreatedAt.HasValue)
			{
				query = query.Where(p => p.CreatedAt <= filter.CreatedAt);
			}

			query = query.Include(p => p.User);
			query = SortParticipants(query, filter.SortBy);

			return await query.ToPagedListAsync(filter.Page, filter.EachPage);
		}

		private static IQueryable<Participant> SortParticipants(IQueryable<Participant> participants, ParticipantSortBy sortBy)
		{
			switch (sortBy)
			{
				//case ParticipantSortBy.Name:
				//	participants = participants.OrderBy(p => p.User.FirstName + p.User.LastName);
				//	break;
				case ParticipantSortBy.CheckedIn:
					participants = participants.OrderByDescending(p => p.CheckedIn);
					break;
				case ParticipantSortBy.CreatedAt:
					participants = participants.OrderBy(p => p.CreatedAt);
					break;
				default:
					// Xử lý trường hợp không hợp lệ (nếu cần)
					break;
			}

			return participants;
		}
	}

}
