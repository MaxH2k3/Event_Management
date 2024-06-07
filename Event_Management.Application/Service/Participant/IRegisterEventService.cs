using Event_Management.Application.Dto.ParticipantDTO;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service
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
	}
}
