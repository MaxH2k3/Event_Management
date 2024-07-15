using Event_Management.Domain.Repository.Common;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Domain.Repository
{
    public interface IFeedbackRepository : IExtendRepository<Feedback>
    {
        Task<PagedList<Feedback>?> GetFeedbackByEventIdAndStar(Guid eventId, int? numOfStar, int page, int eachPage);
        Task<Feedback> GetUserEventFeedback(Guid eventId, Guid userId);
        Task<PagedList<Feedback>?> GetAllUserFeebacks(Guid userId, int page, int eachPage);
    }
}
