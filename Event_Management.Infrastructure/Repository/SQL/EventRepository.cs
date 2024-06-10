﻿using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
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
            var result = await eventList.ToListAsync();
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
            if(!string.IsNullOrWhiteSpace(filter.EventName))
            {
                eventList = from e in eventList
                            where e.EventName.Contains(filter.EventName)
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
            if(filter.StartDateTo != null)
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
            if(filter.TicketFrom != null)
            {
                eventList = from e in eventList
                            where e.Ticket >= filter.TicketFrom
                            select e;
            }
            if (filter.TicketTo != null)
            {
                eventList = from e in eventList
                            where e.Ticket <= filter.TicketTo
                            select e;
            }
            if(filter.Approval != null)
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
        public async Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            //int skipElements = (pageNo - 1) * elementEachPage;
            var events = await GetUserParticipatedEventsQuery(userId);
            var totalEle = await events.CountAsync();
            events =  ApplyFilter(events, filter).PaginateAndSort(pageNo, elementEachPage, filter.SortBy ?? "EventId", filter.IsAscending);
            //events = ApplyFilter(events, filter).Skip(skipElements).Take(elementEachPage);
            var result = await events.ToListAsync();
            return new PagedList<Event>(result, totalEle, pageNo, elementEachPage);
        }

        public async Task<Event> CreateEvent(Event eventCreate)
        {
            await _context.AddAsync(eventCreate);
            await _context.SaveChangesAsync();
            return eventCreate;
        }

        public async void UpdateEventStatusToOnGoing()
        {
            try
            {
                var events = await _context.Events.Where(e => e.StartDate >= DateTime.Now).ToListAsync();
                foreach (var e in events)
                {
                    e.Status = "Ongoing";
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async void UpdateEventStatusToEnded()
        {
            try
            {
                var events = await _context.Events.Where(e => e.EndDate >= DateTime.Now).ToListAsync();
                foreach (var e in events)
                {
                    e.Status = "Ended";
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
