
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Microsoft.AspNetCore.Http;

namespace Event_Management.Application.Service
{
	public interface IEventService
    {
        public Task<APIResponse> GetEventInfo(Guid eventId);
        //public Task<PagedList<EventResponseDto>> GetEventsByTag(int tagId, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetEventsByListTag(List<int> tagIds, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);
        public Task<Dictionary<string, List<EventResponseDto>>> GetUserPastAndFutureEvents(Guid userId);
        public Task<APIResponse> AddEvent(EventRequestDto eventDto, string userId);// HttpContext http);
        public Task<bool> UpdateEvent(EventRequestDto eventDto, string userId);
        public Task<bool> DeleteEvent(Guid eventId);
        public void UpdateEventStatusEnded(Guid eventId);
        public void UpdateEventStatusOngoing(Guid eventId);
        public void UpdateEventStatusEnded();
        public void UpdateEventStatusOngoing();
        public Task<Event?> GetEventById(Guid eventId);
        public List<EventCreatorLeaderBoardDto> GetTop10CreatorsByEventCount();
        public List<EventLocationLeaderBoardDto> GetTop10LocationByEventCount();
        public List<EventCreatorLeaderBoardDto> GetTop20SpeakerEventCount();
        public Dictionary<string, List<EventCreatorLeaderBoardDto>> GetEventLeaderBoards();
        Task<bool> IsOwner(Guid eventId, Guid userId);
    }
        
        
}

