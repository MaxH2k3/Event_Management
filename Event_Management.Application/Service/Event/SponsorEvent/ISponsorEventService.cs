using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
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
        Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage);
    }
}
