using Event_Management.Domain;
using Event_Management.Domain.Repository;
using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class ParticipantRepository : SQLExtendRepository<Participant>, IParticipantRepository
    {
        private readonly EventManagementContext _context;

        public ParticipantRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

		public async Task<bool> IsExistedOnEvent(Guid userId, Guid eventId)
		{
			return await _context.Participants.AnyAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
		}

		public async Task<Participant?> GetParticipant(Guid userId, Guid eventId)
		{
			return await _context.Participants.FirstOrDefaultAsync(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId));
		}

	}
}
