using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Enum;
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

		// Accept register event
		Task<APIResponse> ProcessingTicket(Guid eventId, Guid userId, ParticipantStatus status);

        // Add user to event
        Task<APIResponse> AddToEvent(RegisterEventModel registerEventModel);

        // Get current user on current event
        Task<ParticipantEventModel> GetCurrentUser(Guid userId, Guid eventId);

        // Get all participant related to check-in on event
        Task<PagedList<ParticipantModel>> GetParticipantOnEvent(int page, int eachPage, Guid eventId);


        Task<APIResponse> GetEventParticipants(Guid eventId);

        Task<APIResponse> UserRegisterStatus(Guid eventId, string? userId);
    }
}
