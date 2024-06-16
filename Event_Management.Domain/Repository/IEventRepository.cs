using Event_Management.Domain.Repository.Common;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using Event_Management.Domain.Models.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Domain.Entity;

namespace Event_Management.Domain.Repository
{
    public interface IEventRepository : IExtendRepository<Event>
    {
        //Get List of events that user have participated
        public Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);
        //Get All event with search, paging and sort.
        public Task<PagedList<Event>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);
        //Create Event
        public Task<Event> CreateEvent(Event eventCreate);
        //AUTO update status for event
        //Update status: On going
        public double UpdateEventStatusToOnGoing();
        //Update status: Ended
        public double UpdateEventStatusToEnded();
    }
}
