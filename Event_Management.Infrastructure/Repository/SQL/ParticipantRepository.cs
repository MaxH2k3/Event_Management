using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Extensions;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq.Expressions;

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

        public async Task<Participant?> GetDetailParticipant(Guid userId, Guid eventId)
        {
            return await _context.Participants
				.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
        }

        public async Task<PagedList<Participant>> FilterDataParticipant(FilterParticipant filter)
		{
			var query = _context.Participants.Where(p => p.EventId.Equals(filter.EventId) && p.Status!.Equals(filter.Status.ToString())).AsNoTracking().AsQueryable();

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

        public new async Task<PagedList<Participant>> GetAll(Expression<Func<Participant, bool>> predicate, int page, int eachPage, string sortBy, bool isAscending = true)
        {
            var entities = await _context.Participants
				.Include(p => p.User)
                .Where(predicate)
                .PaginateAndSort(page, eachPage, sortBy, isAscending).ToListAsync();

            return new PagedList<Participant>(entities, entities.Count, page, eachPage);

        }

		public new async Task<IEnumerable<Participant>> GetAll(Expression<Func<Participant, bool>> predicate)
		{
            var entities = await _context.Participants
				.Include(p => p.Event)
                .Include(p => p.User)
				.Where(predicate)
                .ToListAsync();

            return entities;
        }

        private static IQueryable<Participant> SortParticipants(IQueryable<Participant> participants, ParticipantSortBy sortBy)
		{
			switch (sortBy)
			{
				case ParticipantSortBy.CheckedIn:
					participants = participants.OrderByDescending(p => p.CheckedIn);
					break;
				case ParticipantSortBy.CreatedAt:
					participants = participants.OrderBy(p => p.CreatedAt);
					break;
				default:
					break;
			}

			return participants;
		}

		public async Task UpSert(Participant participant)
		{
			var participantExist = _context.Participants.FirstOrDefault(p => p.UserId.Equals(participant.UserId) && p.EventId.Equals(participant.EventId));

			if(participantExist == null)
			{
				await _context.Participants.AddAsync(participant);
            } else
			{
				participantExist.RoleEventId = participant.RoleEventId;
				participantExist.Status = participant.Status;
                _context.Participants.Update(participantExist);
            }

        }

		public async Task<bool> IsRole(Guid userId, Guid eventId, EventRole role)
		{
            return await _context.Participants.AnyAsync(p => p.EventId.Equals(eventId) && p.UserId.Equals(userId) && p.RoleEventId.Equals((int)role));
        }

	}

}
