using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.System;

namespace Event_Management.Domain.Service
{
	public interface IRegisterEventService
	{
		// Update role of participant in event
		Task<APIResponse> UpdateRoleEvent(RegisterEventModel registerEventModel);

		// Register event for user
		Task<APIResponse> RegisterEvent(RegisterEventModel registerEventModel);

		// Delete participant
		Task<APIResponse> DeleteParticipant(Guid userId, Guid eventId);

		// Check in participant when already registered
		Task<APIResponse> CheckInParticipant(Guid userId, Guid eventId);

		// Get all participant on event
		Task<PagedList<ParticipantEventModel>> GetParticipantOnEvent(FilterParticipant filter);
	}
}
