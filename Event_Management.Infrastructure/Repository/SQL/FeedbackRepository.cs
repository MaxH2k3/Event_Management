using Event_Management.Domain;
using Event_Management.Domain.Repository;

using Event_Management.Infrastructure.Repository.Common;
using Event_Management.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Event_Management.Infrastructure.Extensions;

namespace Event_Management.Infrastructure.Repository.SQL
{
    public class FeedbackRepository : SQLExtendRepository<Feedback>, IFeedbackRepository
    {
        private readonly EventManagementContext _context;

        public FeedbackRepository(EventManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedList<Feedback>?> GetFeedbackByEventIdAndStar(Guid eventId, int? numOfStar, int page, int eachPage)
        {
            var list =  _context.Feedbacks.Where(f => f.EventId.Equals(eventId));
            if (numOfStar.HasValue)
            {
                list = list.Where(f => f.Rating == numOfStar.Value);
            }
            list.OrderByDescending(f => f.CreatedAt);

            return await list.ToPagedListAsync(page, eachPage);
        }
    }
}
