﻿using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Repository.Common;

namespace Event_Management.Domain.Repository
{
	public interface IParticipantRepository : IExtendRepository<Participant>
    {
		Task<bool> IsExistedOnEvent(Guid userId, Guid eventId);
		Task<Participant?> GetParticipant(Guid userId, Guid eventId);
		Task<PagedList<Participant>> FilterDataParticipant(FilterParticipant filter);
		Task UpSert(Participant participant);
		Task<Participant?> GetDetailParticipant(Guid userId, Guid eventId);
		Task<bool> IsRole(Guid userId, Guid eventId, EventRole role);

    }
}
