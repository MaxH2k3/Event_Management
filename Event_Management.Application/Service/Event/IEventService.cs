
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Application.Service
{
	public interface IEventService
    {
        public Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);

        public Task<bool> AddEvent(EventRequestDto eventDto);
        public Task<bool> UpdateEvent(EventRequestDto eventDto);
        public Task<bool> DeleteEvent(Guid eventId);

        
    }
        
        
}

