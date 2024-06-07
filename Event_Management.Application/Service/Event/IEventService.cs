
using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service
{
    public interface IEventService
    {
        public Task<APIResponse> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        public Task<APIResponse> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);

        public Task<bool> AddEvent(EventRequestDto eventDto);
        public Task<bool> UpdateEvent(EventRequestDto eventDto);
        public Task<bool> DeleteEvent(Guid eventId);
        Task<APIResponse> GetAllEventTest();

        
    }
        
        
}

