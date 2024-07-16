﻿using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;

namespace Event_Management.Application.Service
{
    public interface ISponsorEventService
    {
        //Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage);
        Task<SponsorEvent> AddSponsorEventRequest(SponsorDto sponsorEvent, Guid userId);
        Task<SponsorEvent> UpdateSponsorEventRequest(SponsorRequestUpdate sponsorRequestUpdate);
        Task<PagedList<SponsorEventDto>> GetSponsorEventsById(SponsorEventFilter sponsorFilter);
        Task<PagedList<SponsorEvent>> GetRequestSponsor(Guid userId, string? status, int page, int eachPage);
        Task<SponsorEvent?> DeleteRequest(Guid eventId, Guid userId);
        Task<SponsorEvent?> GetRequestedDetail(Guid eventId, Guid userId);

		//Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId);
	}
}
