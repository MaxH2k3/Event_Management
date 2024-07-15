using Event_Management.Application.Dto.EventDTO.SponsorDTO;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service
{
    public interface ISponsorEventService
    {
        //Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage);
        Task<SponsorEvent> AddSponsorEventRequest(SponsorDto sponsorEvent);
        Task<SponsorEvent> UpdateSponsorEventRequest(SponsorDto sponsorEvent);
        Task<PagedList<SponsorEventDto>> GetSponsorEventsById(SponsorEventFilter sponsorFilter);
        Task<PagedList<SponsorEvent>> GetSponsoredEvent(Guid userId, int page, int eachPage);

		//Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId);
	}
}
