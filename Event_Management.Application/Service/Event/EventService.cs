using AutoMapper;
using Azure;
using Event_Management.Application.Dto;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Dto.SponsorLogoDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service.FileService;
using Event_Management.Application.Service.Job;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.Service.TagEvent;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Event_Management.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuartzService _quartzService;
        private readonly IImageService _fileService;
        private readonly ITagService _tagService;
        private readonly IUserService _userService;
        private readonly long dateTimeConvertValue = 25200000; //-7h to match JS dateTime type
        public EventService(IUnitOfWork unitOfWork, IMapper mapper, ITagService tagService,
            IQuartzService quartzService, IImageService fileService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tagService = tagService;
            _quartzService = quartzService;
            _fileService = fileService;
            _userService = userService;
        }
        public async Task<APIResponse> GetEventInfo(Guid eventId)
        {
            var eventInfo = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            if(eventInfo != null)
            {
                EventDetailDto eventDetailDto = new EventDetailDto();
                eventDetailDto.EventId = eventId;
                eventDetailDto.EventName = eventInfo.EventName;
                eventDetailDto.Description = eventInfo.Description;
                eventDetailDto.Status = eventInfo.Status;
                eventDetailDto.StartDate = DateTimeHelper.ToJsDateType(eventInfo.StartDate);
                eventDetailDto.EndDate = DateTimeHelper.ToJsDateType((DateTime)eventInfo.EndDate);
                var user = _userService.GetUserById((Guid)eventInfo.CreatedBy!);
                eventDetailDto.Host!.Name = user!.FullName;
                eventDetailDto.Host.avatar = user!.Avatar;
                eventDetailDto.Host.Id = eventInfo.CreatedBy.HasValue ? eventInfo.CreatedBy.Value : null;
                eventDetailDto.Image = eventInfo.Image;
                eventDetailDto.Theme = eventInfo.Theme;
                eventDetailDto.Approval = eventInfo.Approval ? eventInfo.Approval : false;
                eventDetailDto.Capacity = eventInfo.Capacity.HasValue ? eventInfo.Capacity.Value : 0;
                eventDetailDto.CreatedAt = DateTimeHelper.ToJsDateType((DateTime)eventInfo.CreatedAt!);
                eventDetailDto.location!.Name = eventInfo.Location!;
                eventDetailDto.location.Id = eventInfo.LocationId;
                eventDetailDto.location.Coord = eventInfo.LocationCoord;
                eventDetailDto.location.Address = eventInfo.LocationAddress;
                eventDetailDto.location.Url = eventInfo.LocationUrl;
                eventDetailDto.Fare = eventInfo.Fare;
                eventDetailDto.UpdatedAt = eventInfo.UpdatedAt.HasValue ? DateTimeHelper.ToJsDateType(eventInfo.UpdatedAt.Value) : null;
                eventDetailDto.eventTags = _mapper.Map<List<EventTag>>(eventInfo.Tags);
                //eventDetailDto.feedbacks = _mapper.Map<List<FeedbackDto>>(eventInfo.Feedbacks);
                eventDetailDto.sponsorLogos = _mapper.Map<List<SponsorLogoDto>>(eventInfo.Logos);
                //eventDetailDto.participants = _mapper.Map<List<ParticipantInfo>>(eventInfo.Participants);
                return new APIResponse
                {
                    Message = MessageCommon.Complete,
                    StatusResponse = HttpStatusCode.OK,
                    Data = eventDetailDto
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.NotFound,
                StatusResponse = HttpStatusCode.BadRequest,
            };
        }
        public bool IsValidEmail(string email)
        {
            return email.EndsWith("@fpt.edu.vn") || email.EndsWith("@fe.edu.vn") ? true : false;
        }
        private bool IsValidCoordinateString(string coordinateString)
        {
            string pattern = @"^-?\d+(?:\.\d+)?, *-?\d+(?:\.\d+)?$";
            return Regex.IsMatch(coordinateString, pattern);
        }
        public async Task<APIResponse> AddEvent(EventRequestDto eventDto, string userId)// HttpContext context)
        {
            
            bool validate = DateTimeHelper.ValidateStartTimeAndEndTime(eventDto.StartDate, eventDto.EndDate);
            if (!validate)
            {
                return new APIResponse
                {
                    Message = MessageEvent.StartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.EventId = Guid.NewGuid();
            eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate + dateTimeConvertValue).DateTime;
            eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate + dateTimeConvertValue).DateTime;
            if (eventDto.Image != null)
            {
                eventEntity.Image = await _fileService.UploadImage(eventDto.Image, eventEntity.EventId);
            }
            if (string.IsNullOrEmpty(userId))
            {
                return new APIResponse
                {
                    Message = MessageCommon.SessionTimeout,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            else
            {
                var user = await _unitOfWork.UserRepository.GetById(Guid.Parse(userId));
                if (IsValidEmail(user.Email))
                {
                    eventEntity.CreatedBy = Guid.Parse(userId);
                }
                else
                {
                    return new APIResponse
                {
                        Message = MessageEvent.UserNotAllow,
                        StatusResponse = HttpStatusCode.BadRequest
                };
                }
            }
            eventEntity.Fare = eventDto.Ticket;
            eventEntity.CreatedAt = DateTimeHelper.GetDateTimeNow();
            eventEntity.Location = eventDto.Location.Name;
            eventEntity.LocationId = eventDto.Location.Id;
            eventEntity.LocationAddress = eventDto.Location.Address;
            eventEntity.LocationUrl = eventDto.Location.Url;
            if (!IsValidCoordinateString(eventDto.Location.Coord!))
            {
                return new APIResponse 
                {
                    Message = MessageEvent.LocationCoordInvalid,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            eventEntity.LocationCoord = eventDto.Location.Coord;
            eventEntity.Status = EventStatus.NotYet.ToString();
            if(eventDto.TagId.Count > 5)
            {
                return new APIResponse
                {
                    Message = MessageEvent.TagLimitValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
            foreach (int item in eventDto.TagId)
            {
                var tag = await _tagService.GetById(item);
                eventEntity.Tags.Add(tag);
            }
            await _unitOfWork.EventRepository.Add(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.EventId, eventEntity.StartDate);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.EventId, eventEntity.EndDate);
                EventResponseDto response = ToResponseDto(eventEntity);
                return new APIResponse
                {
                    Data = response,
                    Message = MessageCommon.CreateSuccesfully,
                    StatusResponse = HttpStatusCode.OK
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.CreateFailed,
                StatusResponse = HttpStatusCode.BadRequest
            };
        }
        private EventResponseDto ToResponseDto(Event eventEntity)
        { 
            EventResponseDto response = new EventResponseDto();
            response.StartDate = DateTimeHelper.ToJsDateType(eventEntity.StartDate);
            response.EndDate = DateTimeHelper.ToJsDateType(eventEntity.EndDate);
            response.CreatedAt = DateTimeHelper.ToJsDateType((DateTime)eventEntity.CreatedAt!);
            response.Status = eventEntity.Status;
            response.Approval = eventEntity.Approval;
            response.Description = eventEntity.Description;
            response.Location!.Name = eventEntity.Location!;
            response.Location.Id = eventEntity.LocationId;
            response.Location.Coord = eventEntity.LocationCoord;
            response.Location.Address = eventEntity.LocationAddress;
            response.Location.Url = eventEntity.LocationUrl;
            response.EventId = eventEntity.EventId;
            response.EventName = eventEntity.EventName;
            var user = _userService.GetUserById((Guid)eventEntity.CreatedBy!);
            response.Host!.Name = user!.FullName;
            response.Host.Id = eventEntity.CreatedBy.HasValue ? eventEntity.CreatedBy.Value : null;
            response.Host.avatar = user!.Avatar;
            response.Image = eventEntity.Image;
            response.Theme = eventEntity.Theme;
            response.eventTags = _mapper.Map<List<EventTag>>(eventEntity.Tags);
            response.UpdatedAt = eventEntity.UpdatedAt.HasValue ? DateTimeHelper.ToJsDateType(eventEntity.UpdatedAt.Value): null;  
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
        /*public async Task<PagedList<EventResponseDto>> GetEventsByTag(int tagId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetEventsByTag(tagId, pageNo, elementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>();
            response = result.Select(ToResponseDto).ToList();
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;
        }*/
        public async Task<PagedList<EventResponseDto>> GetEventsByListTag(List<int> tagIds, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetEventsByListTags(tagIds, pageNo, elementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>();
            response = result.Select(ToResponseDto).ToList();
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;
        }

        public async Task<APIResponse> UpdateEvent(EventRequestDto eventDto, string userId, Guid eventId)
        {
            //var eventEntity = _mapper.Map<Event>(eventDto);
            var eventEntity = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            if (eventEntity.Status != EventStatus.NotYet.ToString())
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateEventWithStatus,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (string.IsNullOrEmpty(userId))
            {
                return new APIResponse
                {
                    Message = MessageCommon.Unauthorized,
                    StatusResponse = HttpStatusCode.Unauthorized,
                };
            }
            var userinfo = await _unitOfWork.UserRepository.GetById(Guid.Parse(userId));
            if(!userId.Equals(eventEntity.CreatedBy.Value.ToString()))
            {
                return new APIResponse
                {
                    Message = MessageEvent.OnlyHostCanUpdateEvent,
                    StatusResponse = HttpStatusCode.Unauthorized,
                };
            }
            eventEntity.UpdatedAt = DateTimeHelper.GetDateTimeNow();
            //name
            eventEntity.EventName = eventDto.EventName;
            //description
            eventEntity.Description = eventDto.Description;
            //tags;
            eventEntity.Tags.Clear();
            foreach (int tagId in eventDto.TagId)
            {
                Tag tag = await _unitOfWork.TagRepository.GetById(tagId);
                eventEntity.Tags.Add(tag!);
            }
            //startDate
            if (eventDto.StartDate > 0 && eventDto.StartDate - DateTimeHelper.ToJsDateType((DateTime)eventEntity.CreatedAt!) > 0)
            eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate + +dateTimeConvertValue).DateTime;
            //endDate
            if(eventDto.EndDate > 0 && eventDto.EndDate - eventDto.StartDate >= 30*60*1000)
            eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate + +dateTimeConvertValue).DateTime;
            //theme
            eventEntity.Theme = eventDto.Theme;
            //image
            if (!string.IsNullOrWhiteSpace(eventDto.Image))
            {
                if(await _fileService.DeleteBlob(eventEntity.EventId.ToString()))
                eventEntity.Image = await _fileService.UploadImage(eventDto.Image, eventEntity.EventId);
            }
            //location
            eventEntity.Location = eventDto.Location!.Name;
            eventEntity.LocationId = eventDto.Location.Id;
            eventEntity.LocationCoord = eventDto.Location.Coord;
            eventEntity.LocationAddress = eventDto.Location.Address;
            eventEntity.LocationUrl = eventDto.Location.Url;
            //capacity
            eventEntity.Capacity = eventDto.Capacity;
            //approval
            eventEntity.Approval = eventDto.Approval.HasValue ? eventDto.Approval.Value : false;
            //fare / ticket
            eventEntity.Fare = eventDto.Ticket;
            await _unitOfWork.EventRepository.Update(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse
                {
                    Message = MessageCommon.UpdateSuccesfully,
                    StatusResponse = HttpStatusCode.OK,
                    Data = ToResponseDto(eventEntity)
                };
            }
            return new APIResponse
            {
                Message = MessageCommon.UpdateFailed,
                StatusResponse = HttpStatusCode.BadRequest,
            };
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

        public async Task<bool> IsOwner(Guid eventId, Guid userId)
        {
            return await _unitOfWork.EventRepository.IsOwner(userId, eventId);
        }
    }
}
