using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;


namespace Event_Management.Application.Service
{
	public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

		public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddEvent(EventRequestDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.CreatedAt = DateTime.Now;
            eventEntity.UpdatedAt = DateTime.Now;
            await _unitOfWork.EventRepository.Add(eventEntity);
            return await _unitOfWork.SaveChangesAsync();
          
           

        }

        public async Task<bool> DeleteEvent(Guid eventId)
        {
            await _unitOfWork.EventRepository.Delete(eventId);
            return await _unitOfWork.SaveChangesAsync();
        }

		//	_distributedCache = distributedCache;
		//}
        public async Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage)
        {
            
            var result = await _unitOfWork.EventRepository.GetAllEvents(filter, pageNo, elementEachPage);
            List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            
                PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                    (response, result.TotalItems, pageNo, elementEachPage);
                return pages;
        }

        public async Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
                PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                    (response, response.Count, pageNo, elementEachPage);
                return pages;
            
        }

        public async Task<bool> UpdateEvent(EventRequestDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);

            eventEntity.UpdatedAt = DateTime.Now;
            await _unitOfWork.EventRepository.Update(eventEntity);
            return await _unitOfWork.SaveChangesAsync();


        }

       
    }
}
