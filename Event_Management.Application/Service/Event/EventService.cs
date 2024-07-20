using AutoMapper;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.SponsorLogoDTO;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Application.Message;
using Event_Management.Application.Service.FileService;
using Event_Management.Application.Service.Job;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Event;
using Event_Management.Domain.Models.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Net;
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
        private readonly long minimumUpdateTimeSpan = 21600000;//time span between event created and new event startDate
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
            /*await _quartzService.StartEventStartingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
            await _quartzService.StartEventEndingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
            await _quartzService.DeleteJobsByEventId("start-" + eventId);
            await _quartzService.DeleteJobsByEventId("ended-" + eventId);*/
            if(eventInfo!.Status!.Equals(EventStatus.Deleted.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new APIResponse
                {
                    Message = MessageCommon.NotFound,
                    StatusResponse = HttpStatusCode.NotFound,
                };
            }
            if (eventInfo != null)
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
                eventDetailDto.Host.email = user!.Email!;
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
            var tempStartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate).DateTime;
            var tempEndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate).DateTime;
            var TimeSpan = tempEndDate - tempStartDate;
            if (tempStartDate > DateTime.Now.AddMonths(4) || TimeSpan.TotalDays > 7)
            {
                return new APIResponse
                {
                    Message = MessageEvent.StarTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest
                };
            }
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
                eventEntity.Image = await _fileService.UploadImage(eventDto.Image, Guid.NewGuid());
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
            if (eventDto.TagId.Count > 5)
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
                await _quartzService.StartEventStartingEmailNoticeJob(eventEntity.EventId, eventEntity.StartDate.AddHours(-1));
                await _quartzService.StartEventEndingEmailNoticeJob(eventEntity.EventId, eventEntity.EndDate.AddHours(1));
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
        private CreatedByUserDto getHostInfo(Guid userId)
        {
            var user = _userService.GetUserById(userId);
            CreatedByUserDto response = new CreatedByUserDto();
            response.avatar = user!.Avatar;
            response.Id = user.UserId;
            response.Name = user.FullName;
            return response;
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
            response.Host = getHostInfo((Guid)eventEntity.CreatedBy!);
            response.Image = eventEntity.Image;
            response.Theme = eventEntity.Theme;
            response.eventTags = _mapper.Map<List<EventTag>>(eventEntity.Tags);
            response.UpdatedAt = eventEntity.UpdatedAt.HasValue ? DateTimeHelper.ToJsDateType(eventEntity.UpdatedAt.Value) : null;
            response.Fare = eventEntity.Fare;
            response.Capacity = eventEntity.Capacity;
            return response;
        }
        private EventPreview ToEventPreview(Event entity)
        {
            EventPreview response = new EventPreview();
            response.EventId = entity.EventId;
            response.EventName = entity.EventName;
            response.Location = entity.Location;
            response.Status = entity.Status;
            response.Image = entity.Image;
            response.StartDate = DateTimeHelper.ToJsDateType(entity.StartDate);
            response.Host = getHostInfo((Guid)entity.CreatedBy!);
            return response;
        }
        private async Task<bool> IsDeletable(Guid eventId)
        {
            Event? existEvent = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
            bool isSponsored = await _unitOfWork.SponsorEventRepository.IsSponsored(eventId);
            var participantList = existEvent.Participants.Where(p => p.Status!.Equals(ParticipantStatus.Confirmed.ToString())).ToList();

            return existEvent!.Fare <= 0 && !participantList.Any() && !existEvent.Status!.Equals(EventStatus.OnGoing) 
                || !isSponsored && !existEvent.Status!.Equals(EventStatus.OnGoing);
        }
        public async Task<bool> DeleteEvent(Guid eventId, Guid userId)
        {
            try
            {
                bool isOwner = await IsOwner(eventId, userId);
                bool isDeletable = await IsDeletable(eventId);
                var userInfo = await _unitOfWork.UserRepository.GetById(userId);
                Event? existEvent = await _unitOfWork.EventRepository.getAllEventInfo(eventId);
                if (existEvent == null || !existEvent.Status!.Equals(EventStatus.NotYet.ToString()) || !isDeletable)
                {
                    return false;
                }
                if (isOwner)
                {
                    if (existEvent.StartDate.CompareTo(DateTime.Now.AddHours(6)) < 0)
                    {
                        return await _unitOfWork.EventRepository.ChangeEventStatus(eventId, EventStatus.Cancel);
                    }
                    return await _unitOfWork.EventRepository.ChangeEventStatus(eventId, EventStatus.Deleted);
                }
                if (!isOwner && userInfo!.RoleId == ((int)UserRole.Admin))
                {
                    return await _unitOfWork.EventRepository.ChangeEventStatus(eventId, EventStatus.Aborted);
                }
            }catch (Exception)
            {
                return false;
            }
            return false;
        }
        public async Task<Dictionary<string, List<EventPreview>>> GetUserPastAndFutureEvents(Guid userId)
        {
            List<Event> pastEvent = await _unitOfWork.EventRepository.UserPastEvents(userId);
            List<Event> incoming = await _unitOfWork.EventRepository.UserIncomingEvents(userId);
            Dictionary<string, List<EventPreview>> response = new Dictionary<string, List<EventPreview>>
            {
                { "IncomingEvent", incoming.Select(ToEventPreview).ToList() },
                { "PastEvent", pastEvent.Select(ToEventPreview).ToList() }
            };
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
            response = result.Select(ToResponseDto).ToList();
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
            if (!userId.Equals(eventEntity.CreatedBy!.Value.ToString()))
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
            if (eventDto.StartDate > 0 && eventDto.StartDate - (DateTimeHelper.ToJsDateType((DateTime)eventEntity.CreatedAt!)) < minimumUpdateTimeSpan)
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateStartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (eventDto.StartDate > 0 && eventDto.StartDate - (DateTimeHelper.ToJsDateType((DateTime)eventEntity.CreatedAt!)) >= minimumUpdateTimeSpan)
            {
                eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate + dateTimeConvertValue).DateTime;
                await _quartzService.DeleteJobsByEventId("start-" + eventEntity.EventId);
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.EventId, eventEntity.StartDate);
                await _quartzService.StartEventStartingEmailNoticeJob(eventEntity.EventId, eventEntity.StartDate.AddHours(-1));
            }
            //endDate
            if (eventDto.EndDate > 0 && eventDto.EndDate - DateTimeHelper.ToJsDateType(eventEntity.StartDate) < 30 * 60 * 1000)
            {
                return new APIResponse
                {
                    Message = MessageEvent.UpdateStartEndTimeValidation,
                    StatusResponse = HttpStatusCode.BadRequest,
                };
            }
            if (eventDto.EndDate > 0 && eventDto.EndDate - DateTimeHelper.ToJsDateType(eventEntity.StartDate) >= 30 * 60 * 1000)
            {
                eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate + dateTimeConvertValue).DateTime;
                await _quartzService.DeleteJobsByEventId("ended-" + eventEntity.EventId);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.EventId, eventEntity.EndDate);
                await _quartzService.StartEventEndingEmailNoticeJob(eventEntity.EventId, eventEntity.EndDate.AddHours(1));
            }
            //theme
            eventEntity.Theme = eventDto.Theme;
            //image
            if (!string.IsNullOrWhiteSpace(eventDto.Image))
            {
                string url = eventEntity.Image!;
                int startIndex = url.LastIndexOf("/eventcontainer/") + "/eventcontainer/".Length;
                string result = url.Substring(startIndex);
                if (await _fileService.DeleteBlob(result))
                    eventEntity.Image = await _fileService.UploadImage(eventDto.Image, Guid.NewGuid());
            }
            //location
            eventEntity.Location = eventDto.Location!.Name;
            eventEntity.LocationId = eventDto.Location.Id;
            eventEntity.LocationCoord = eventDto.Location.Coord;
            eventEntity.LocationAddress = eventDto.Location.Address;
            eventEntity.LocationUrl = eventDto.Location.Url;
            //capacity
            eventEntity.Capacity = eventDto.Capacity.HasValue ? eventDto.Capacity.Value : eventEntity.Capacity;
            //approval
            eventEntity.Approval = eventDto.Approval.HasValue ? eventDto.Approval.Value : eventEntity.Approval;
            //fare / ticket
            eventEntity.Fare = eventDto.Ticket.HasValue ? eventDto.Ticket.Value : eventEntity.Fare;
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
            //var top20Speaker = GetTop20SpeakerEventCount();
            result.Add("eventCreator", top10EventCreator);
            //result.Add("top 20 event speaker", top20Speaker);
            return result;
        }
        public async Task<PagedList<EventResponseDto>> GetEventByUserRole(EventRole eventRole, string userId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.getEventByUserRole(eventRole, Guid.Parse(userId), pageNo, elementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>();
            response = result.Select(ToResponseDto).ToList();
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;
        }
        public async Task<bool> IsOwner(Guid eventId, Guid userId)
        {
            return await _unitOfWork.EventRepository.IsOwner(userId, eventId);
        }
        public async Task<EventStatistics?> GetEventStatis(Guid eventId)
        {
            return await _unitOfWork.EventStatisticsRepository.GetById(eventId);
        }
        public async Task<Dictionary<string, int>> CountByStatus()
        {
            return await _unitOfWork.EventRepository.CountByStatus();
        }
        public async Task<List<EventPreview>> GetUserHostEvent(Guid userId)
        {
            var result = await _unitOfWork.EventRepository.GetUserHostEvent(userId);
            return result.Select(ToEventPreview).ToList();
        }
        public async Task<List<EventPerMonth>> EventsPerMonth(DateTime startDate, DateTime endDate)
        {
            var result = await _unitOfWork.EventRepository.EventsPerMonth(startDate, endDate);
            return result;
        }
    }
}
