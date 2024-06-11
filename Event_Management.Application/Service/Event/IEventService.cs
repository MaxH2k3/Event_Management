
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;

namespace Event_Management.Application.Service
{
	public interface IEventService
    {
        public Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        public Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);
        public Task<string?> UploadImage(FileUploadDto dto);
        public Task<List<string>> GetAllBlobUris();
        public Task<string?> GetBlobUri(string blobName);
        public Task<Event> AddEvent(EventRequestDto eventDto);
        public Task<bool> UpdateEvent(EventRequestDto eventDto);
        public Task<bool> DeleteEvent(Guid eventId);

        
    }
        
        
}

