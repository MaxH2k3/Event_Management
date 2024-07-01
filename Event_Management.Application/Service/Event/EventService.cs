using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service.FileService;
using Event_Management.Application.Service.Job;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;


namespace Event_Management.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IQuartzService _quartzService;
        private readonly IImageService _fileService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, 
            IConfiguration configuration, IQuartzService quartzService, IImageService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
            _quartzService = quartzService;
            _fileService = fileService;
        }

        public async Task<EventResponseDto> AddEvent(EventRequestDto eventDto, string userId)// HttpContext context)
        {
            
            bool validate = DateTimeHelper.ValidateStartTimeAndEndTime(eventDto.StartDate, eventDto.EndDate);
            if (!validate)
            {
                throw new InvalidOperationException(MessageEvent.StartEndTimeValidation);
            }
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.EventId = Guid.NewGuid();
            eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate).DateTime;
            eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate).DateTime;
            if (eventDto.Image != null)
            {
                eventEntity.Image = await _fileService.UploadImage(eventDto.Image, eventEntity.EventId);
            }
            //string userId = IndentityExtension.GetUserIdFromToken2(context);
            //string userId = IndentityExtension.GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception(MessageCommon.SessionTimeout);
            }
            else
            {
                eventEntity.CreatedBy = Guid.Parse(userId);
            }

            eventEntity.CreatedAt = DateTime.Now;
            eventEntity.Location = eventDto.Location.Location;
            eventEntity.LocationId = eventDto.Location.LocationId;
            eventEntity.LocationAddress = eventDto.Location.LocationAddress;
            eventEntity.LocationUrl = eventDto.Location.LocationUrl;
            eventEntity.LocationCoord = eventDto.Location.LocationCoord;
            eventEntity.Status = EventStatus.NotYet.ToString();
            await _unitOfWork.EventRepository.Add(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.EventId, eventEntity.StartDate);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.EventId, eventEntity.EndDate);
                EventResponseDto response = ToResponseDto(eventEntity);
                return response;
            }
            throw new Exception(MessageCommon.CreateFailed);
        }
        private EventResponseDto ToResponseDto(Event eventEntity)
        { 
            long epochTime = DateTimeHelper.epochTime;
            EventResponseDto response = new EventResponseDto();
            response.StartDate = DateTimeHelper.ToJsDateType(eventEntity.StartDate);
            response.EndDate = DateTimeHelper.ToJsDateType(eventEntity.EndDate);
            response.CreatedAt = DateTimeHelper.ToJsDateType((DateTime)eventEntity.CreatedAt!);
            response.Status = eventEntity.Status;
            response.Approval = eventEntity.Approval;
            response.Description = eventEntity.Description;
            response.Location = eventEntity.Location;
            response.LocationId = eventEntity.LocationId;
            response.LocationCoord = eventEntity.LocationCoord;
            response.LocationAddress = eventEntity.LocationAddress;
            response.LocationUrl = eventEntity.LocationUrl;
            response.EventId = eventEntity.EventId;
            response.EventName = eventEntity.EventName;
            response.CreatedBy = eventEntity.CreatedBy;
            response.Image = eventEntity.Image;
            response.Theme = eventEntity.Theme;
            if(eventEntity.UpdatedAt.ToString() != null)
            {
                response.UpdatedAt = DateTimeHelper.ToJsDateType((DateTime)eventEntity.UpdatedAt!);
            }
            else
            {
                response.UpdatedAt = null;
            }
            response.Fare = eventEntity.Fare;
            response.Capacity = eventEntity.Capacity;
            return response;
        }
        public async Task<bool> DeleteEvent(Guid eventId)
        {
            Event? existEvent = await _unitOfWork.EventRepository.GetById(eventId);
            if (existEvent == null || existEvent.Status.Equals(EventStatus.OnGoing))
            {
                return false;
            }
            return await _unitOfWork.EventRepository.DeleteEvent(eventId);
        }

        //	_distributedCache = distributedCache;
        //}
        public async Task<Dictionary<string, List<EventResponseDto>>> GetUserPastAndFutureEvents(Guid userId)
        {
            List<Event> pastEvent = await _unitOfWork.EventRepository.UserPastEvents(userId);
            List<Event> incoming = await _unitOfWork.EventRepository.UserIncomingEvents(userId);
            Dictionary<string, List<EventResponseDto>> response = new Dictionary<string, List<EventResponseDto>>();
            response.Add("IncomingEvent", incoming.Select(ToResponseDto).ToList());
            response.Add("PastEvent", pastEvent.Select(ToResponseDto).ToList());
            return response;
        }
        public async Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage)
        {

            var result = await _unitOfWork.EventRepository.GetAllEvents(filter, pageNo, elementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>(); //_mapper.Map<List<EventResponseDto>>(result);
            response = result.Select(ToResponseDto).ToList();
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, result.TotalItems, pageNo, elementEachPage);
            return pages;
        }

        public async Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            //List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            List<EventResponseDto> response = new List<EventResponseDto>();
            response = result.Select (ToResponseDto).ToList();
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;

        }

        public async Task<bool> UpdateEvent(EventRequestDto eventDto, string userId)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            if (!string.IsNullOrEmpty(userId))
            {
                eventEntity.CreatedBy = Guid.Parse(userId);
            }
            eventEntity.UpdatedAt = DateTime.Now;
            eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate).DateTime;
            eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate).DateTime;
            await _unitOfWork.EventRepository.Update(eventEntity);
            return await _unitOfWork.SaveChangesAsync();


        }
        public void UpdateEventStatusEnded(Guid eventId)
        {
            _unitOfWork.EventRepository.UpdateEventStatusToEnded(eventId);
        }
        public void UpdateEventStatusOngoing(Guid eventId)
        {
            _unitOfWork.EventRepository.UpdateEventStatusToOnGoing(eventId);
        }
        public void UpdateEventStatusEnded()
        {
            _unitOfWork.EventRepository.UpdateEventStatusToEnded();
        }
        public void UpdateEventStatusOngoing()
        {
            _unitOfWork.EventRepository.UpdateEventStatusToOnGoing();
        }
        public async Task<Event?> GetEventById(Guid eventId)
        {
            return await _unitOfWork.EventRepository.GetById(eventId);
        }
        public List<EventCreatorLeaderBoardDto> GetTop10CreatorsByEventCount()
        {
            return _unitOfWork.EventRepository.GetTop10CreatorsByEventCount();
        }
        public List<EventLocationLeaderBoardDto> GetTop10LocationByEventCount()
        {
            return _unitOfWork.EventRepository.GetTop10LocationByEventCount();
        }
        public List<EventCreatorLeaderBoardDto> GetTop20SpeakerEventCount()
        {
            return _unitOfWork.EventRepository.GetTop20SpeakerEventCount();
        }
        public Dictionary<string, List<EventCreatorLeaderBoardDto>> GetEventLeaderBoards()
        {
            Dictionary<string, List<EventCreatorLeaderBoardDto>> result = new Dictionary<string, List<EventCreatorLeaderBoardDto>>();
            var top10EventCreator = GetTop10CreatorsByEventCount();
            var top20Speaker = GetTop20SpeakerEventCount();
            result.Add("top 10 event creator", top10EventCreator);
            result.Add("top 20 event speaker", top20Speaker);
            return result;
        }
    }
}
