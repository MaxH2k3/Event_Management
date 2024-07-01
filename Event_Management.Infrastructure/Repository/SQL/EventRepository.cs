using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Extensions;
using Event_Management.Infrastructure.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Globalization;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class EventRepository : SQLExtendRepository<Event>, IEventRepository
    {
        private readonly EventManagementContext _context;

        public EventRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedList<Event>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage)
        {
            //int skipElements = (pageNo - 1) * elementEachPage;
            var totalEle = await _context.Events.CountAsync();
            var eventList = _context.Events.AsQueryable();
            eventList = ApplyFilter(eventList, filter).PaginateAndSort(pageNo, elementEachPage, filter.SortBy ?? "EventId", filter.IsAscending);
            //eventList = ApplyFilter(eventList, filter).Skip(skipElements).Take(elementEachPage);
            List<Event> temp = await eventList.ToListAsync();
            List<Event> result = new List<Event>();
            if (!string.IsNullOrWhiteSpace(filter.UserCoord))
            {
                result = FilterEventsByDistance(temp, filter.UserCoord);
            }
            else
            {
                result = temp;
            }
            return new PagedList<Event>(result, totalEle, pageNo, elementEachPage);
        }
        private async Task<List<Event>?> ApplySearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.Events.ToListAsync();

            var result = await _context.Events
                .Where(e => e.EventName.Contains(keyword) || e.Location!.Contains(keyword))
                .ToListAsync();

            return result.Any() ? result : null;
        }
        private IQueryable<Event> ApplyFilter(IQueryable<Event> eventList, EventFilterObject filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.EventName))
            
            {
                    eventList = from e in eventList
                                where e.EventName!.Contains(filter.EventName)
                                select e;
            }
            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                eventList = from e in eventList
                            where e.Location!.Contains(filter.Location)
                            select e;
            }
            if (filter.StartDateFrom != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.StartDateFrom);
                // get DateTime from DateTimeOffset
                DateTime startDateFrom = dateTimeOffset.DateTime;
                eventList = from e in eventList
                            where e.StartDate >= startDateFrom
                            select e;
            }
            if (filter.StartDateTo != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.StartDateTo);
                // get DateTime from DateTimeOffset
                DateTime startDateTo = dateTimeOffset.DateTime;
                eventList = from e in eventList
                            where e.StartDate <= startDateTo
                            select e;
            }
            if (filter.EndDateFrom != null)
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.EndDateFrom);
                // get DateTime from DateTimeOffset
                DateTime endDateFrom = dateTimeOffset.DateTime;
                eventList = from e in eventList
                            where e.EndDate >= endDateFrom
                            select e;

            }
            if ((filter.EndDateTo != null))
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)filter.EndDateTo);
                // get DateTime from DateTimeOffset
                DateTime endDateTo = dateTimeOffset.DateTime;
                eventList = from e in eventList
                            where e.EndDate <= endDateTo
                            select e;
            }
            {

            }
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                eventList = from e in eventList
                            where e.Status == filter.Status
                            select e;
            }
            if (filter.TicketFrom != null)
            {
                eventList = from e in eventList
                            where e.Fare >= filter.TicketFrom
                            select e;
            }
            if (filter.TicketTo != null)
            {
                eventList = from e in eventList
                            where e.Fare <= filter.TicketTo
                            select e;
            }
            if (filter.Approval != null)
            {
                eventList = from e in eventList
                            where e.Approval == filter.Approval
                            select e;
            }

            //eventList = filter.IsAscending ? eventList.OrderBy(s => s.EndDate) : eventList.OrderByDescending(s => s.StartDate);               
            return eventList;
        }
        private async Task<IQueryable<Event>> GetUserParticipatedEventsQuery(string userId)
        {
            var participants = await _context.Participants
                .Where(p => p.UserId.Equals(userId) && p.CheckedIn != null)
                .Select(p => p.EventId)
                .ToListAsync();

            return _context.Events
                .Where(e => participants.Contains(e.EventId))
                .AsNoTracking();
        }
        private async Task<IQueryable<Event>> GetUserRegisterdEventsQuery(Guid userId)
        {
            var participants = await _context.Participants
                .Where(p => p.UserId.Equals(userId))
                .Select(p => p.EventId)
                .ToListAsync();

            return _context.Events
                .Where(e => participants.Contains(e.EventId))
                .AsNoTracking();
        }
        public async Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            //int skipElements = (pageNo - 1) * elementEachPage;
            var events = await GetUserParticipatedEventsQuery(userId);
            var totalEle = await events.CountAsync();
            events = ApplyFilter(events, filter).PaginateAndSort(pageNo, elementEachPage, filter.SortBy ?? "EventId", filter.IsAscending);
            //events = ApplyFilter(events, filter).Skip(skipElements).Take(elementEachPage);
            var result = await events.ToListAsync();
            return new PagedList<Event>(result, totalEle, pageNo, elementEachPage);
        }
        public async Task<PagedList<Event>> GetAllEventsByStatus(EventFilterObject filter, int pageNo, int elementEachPage, EventStatus status)
        {
            var totalEle = await _context.Events.CountAsync();
            var eventList = _context.Events.Where(e => e.Status == status.ToString()).AsQueryable();
            eventList = ApplyFilter(eventList, filter).PaginateAndSort(pageNo, elementEachPage, filter.SortBy ?? "EventId", filter.IsAscending);
            List<Event> result = new List<Event>();
            List<Event> temp = await eventList.ToListAsync();
            if (!string.IsNullOrWhiteSpace(filter.UserCoord))
            {
                result = FilterEventsByDistance(temp, filter.UserCoord);
            }
            else
            {
                result = temp;
            }
            return new PagedList<Event>(result, totalEle, pageNo, elementEachPage);
        }
        public async Task<Event> CreateEvent(Event eventCreate)
        {
            await _context.AddAsync(eventCreate);
            await _context.SaveChangesAsync();
            return eventCreate;
        }

        public async Task<List<Event>> UserPastEvents(Guid userId)
        {
            var events = await GetUserRegisterdEventsQuery(userId);
            return events.Where(e => e.EndDate.Date < DateTime.Now)
                
                .OrderByDescending(e => e.EndDate)
                .ToList();
        }
        public async Task<List<Event>> UserIncomingEvents(Guid userId)
        {
            var incomingEvents = await GetUserRegisterdEventsQuery(userId);
            return incomingEvents.Where(e => e.StartDate.Date >= DateTime.Now)
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        public bool UpdateEventStatusToOnGoing(Guid eventId)
        {
            var ongoingEvent = _context.Events.Find(eventId);
            ongoingEvent.Status = EventStatus.OnGoing.ToString();
            _context.Update(ongoingEvent);
            /*var ongoingEvents = _context.Events.Where(e => e.StartDate <= DateTime.Now).ToList();
            ongoingEvents.ForEach(e => e.Status = EventStatus.OnGoing.ToString());
                _context.UpdateRange(ongoingEvents);*/
               return _context.SaveChanges() > 0;
        }

        public bool UpdateEventStatusToEnded(Guid eventId)
        {
            var endedEvent = _context.Events.Find(eventId);
            endedEvent.Status = EventStatus.Ended.ToString();
            _context.Update(endedEvent);
            /*var endedEvents = _context.Events.Where(e => e.EndDate <= DateTime.Now).ToList();
            endedEvents.ForEach(e => e.Status = EventStatus.Ended.ToString());
            _context.UpdateRange(endedEvents);*/
                return _context.SaveChanges() > 0;
        }
        public bool UpdateEventStatusToOnGoing()
        {
            var ongoingEvents = _context.Events.Where(e => e.StartDate <= DateTime.Now &&
            e.Status!.Equals(EventStatus.NotYet.ToString())).ToList();
            ongoingEvents.ForEach(e => e.Status = EventStatus.OnGoing.ToString());
                _context.UpdateRange(ongoingEvents);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateEventStatusToEnded()
        {
            var endedEvents = _context.Events.Where(e => e.EndDate <= DateTime.Now &&
            e.Status!.Equals(EventStatus.OnGoing.ToString())).ToList();
            endedEvents.ForEach(e => e.Status = EventStatus.Ended.ToString());
            _context.UpdateRange(endedEvents);
            return _context.SaveChanges() > 0;
        }
        public async Task<bool> DeleteEvent(Guid eventId)
        {
            var deleteEvent = await _context.Events.FindAsync(eventId);
            deleteEvent.Status = EventStatus.Deleted.ToString();
            return await _context.SaveChangesAsync() > 0;
        }
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371.0; // Earth's radius in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;
            return distance * 1000;//convert from kilometer to meter
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private List<Event> FilterEventsByDistance(List<Event> events, string userCoord)
        {
            List<Event> result = new List<Event>();

            if (!string.IsNullOrWhiteSpace(userCoord))
            {
                string[] userCoordParts = userCoord.Split(',');
                double userLat = double.Parse(userCoordParts[0].Trim());
                double userLong = double.Parse(userCoordParts[1].TrimStart());

                foreach (var item in events)
                {
                    string[] eventCoordParts = item.LocationCoord!.Split(',');
                    double eventLat = double.Parse(eventCoordParts[0].Trim());
                    double eventLong = double.Parse(eventCoordParts[1].TrimStart());
                    double distance = CalculateDistance(userLat, userLong, eventLat, eventLong);

                    if (distance <= 5000)
                    {
                        Console.WriteLine($"event {item.EventName} distance: " + distance);
                        result.Add(item);
                    }
                }
            }
            else
            {
                result = events.ToList();
            }

            return result;
        }
        public List<EventCreatorLeaderBoardDto> GetTop10CreatorsByEventCount()
        {
            List<EventCreatorLeaderBoardDto> userInfos = new List<EventCreatorLeaderBoardDto>(); 
            var result = _context.Events
                .AsEnumerable()
                .GroupBy(e => e.CreatedBy!)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => _context.Users.Find(g.Key)!.UserId.ToString(), g => g.Count());
            foreach (var item in result)
            {
                var temp = _context.Users.Find(Guid.Parse(item.Key!));
                var userInfo = new EventCreatorLeaderBoardDto();
                userInfo.totalevent = item.Value;
                userInfo.FullName = temp.FullName;
                userInfo.Avatar = temp.Avatar;
                userInfo.userId = temp.UserId;
                userInfos.Add(userInfo);
            }
            return userInfos;
        }
        public List<EventLocationLeaderBoardDto> GetTop10LocationByEventCount()
        {
            List<EventLocationLeaderBoardDto> result = new List<EventLocationLeaderBoardDto>();
            var temp = _context.Events
                .AsEnumerable()
                .GroupBy(e => e.Location!)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToDictionary(g => g.Key, g => g.Count());
            foreach (var item in temp)
            {
                EventLocationLeaderBoardDto locationInfo = new EventLocationLeaderBoardDto();
                var eventTemp = _context.Events.FirstOrDefault(e => e.Location.Equals(item.Key));
                locationInfo.totalevent = item.Value;
                locationInfo.Location = item.Key;
                locationInfo.LocationId = eventTemp.LocationId;
                locationInfo.LocationUrl = eventTemp.LocationUrl;
                locationInfo.LocationCoord = eventTemp.LocationCoord;
                locationInfo.LocationAddress = eventTemp.LocationAddress;
                result.Add(locationInfo);
            }
            return result!;
        }
        public List<EventCreatorLeaderBoardDto> GetTop20SpeakerEventCount()
        {
            List<EventCreatorLeaderBoardDto> userInfos = new List<EventCreatorLeaderBoardDto>();
            var result = _context.Participants
                .AsEnumerable()
                .Where(p => p.RoleEventId == 1)
                .GroupBy(p => p.UserId)
                .OrderByDescending(g => g.Count())
                .Take(20)
                .ToDictionary(g => _context.Users.Find(g.Key)!.UserId.ToString(), g => g.Count());
            foreach (var item in result)
            {
                var temp = _context.Users.Find(Guid.Parse(item.Key!));
                var userInfo = new EventCreatorLeaderBoardDto();
                userInfo.totalevent = item.Value;
                userInfo.FullName = temp.FullName;
                userInfo.Avatar = temp.Avatar;
                userInfo.userId = temp.UserId;
                userInfos.Add(userInfo);
            }
            return userInfos;
                
        }
    }
}
