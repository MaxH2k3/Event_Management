using Event_Management.Domain.Repository.Common;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;

namespace Event_Management.Domain.Repository
{
    public interface IEventRepository : IExtendRepository<Event>
    {
        //Get List of events that user have participated
        public Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage);

        //Get All event with search, paging and sort.
        public Task<PagedList<Event>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage);

        //Get eventList by status 
        public Task<PagedList<Event>> GetAllEventsByStatus(EventFilterObject filter, int pageNo, int elementEachPage, EventStatus status);

        //Create Event
        public Task<Event> CreateEvent(Event eventCreate);


        //AUTO update status for event
        //Update status: On going
        public bool UpdateEventStatusToOnGoing();
        //Update status: Ended
        public bool UpdateEventStatusToEnded();


        public Task<bool> DeleteEvent(Guid eventId);
    }
}
