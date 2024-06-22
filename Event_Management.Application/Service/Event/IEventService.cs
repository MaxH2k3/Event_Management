
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
        public Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);
        public Task<string?> UploadImage(FileUploadDto dto);
        public Task<List<string>> GetAllBlobUris();
        public Task<string?> GetBlobUri(string blobName);
        public Task<EventResponseDto> AddEvent(EventRequestDto eventDto, string userId);// HttpContext http);
        public Task<bool> UpdateEvent(EventRequestDto eventDto, string userId);
        public Task<bool> DeleteEvent(Guid eventId);
        public void UpdateEventStatusEnded();
        public void UpdateEventStatusOngoing();

        public Task<Event?> GetEventById(Guid eventId);

        
    }
        
        
}

