using Event_Management.Domain.Repository.Common;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.Event;

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
        //User past and incoming events
        public Task<List<Event>> UserPastEvents(Guid userId);
        public Task<List<Event>> UserIncomingEvents(Guid userId);

        //AUTO update status for event
        //Update status: On going
        public bool UpdateEventStatusToOnGoing(Guid eventId);
        public bool UpdateEventStatusToOnGoing();
        //Update status: Ended
        public bool UpdateEventStatusToEnded(Guid eventId);
        public bool UpdateEventStatusToEnded();

        Task<bool> ChangeEventStatus(Guid eventId, EventStatus status);
        public List<EventCreatorLeaderBoardDto> GetTop10CreatorsByEventCount();
        public List<EventLocationLeaderBoardDto> GetTop10LocationByEventCount();
        public List<EventCreatorLeaderBoardDto> GetTop20SpeakerEventCount();
        Task<bool> IsOwner(Guid userId, Guid eventId);

        // get Event
        public Task<Event> getAllEventInfo(Guid eventId);
        public Task<PagedList<Event>> getEventByUserRole(EventRole eventRole, Guid userId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByTag(int tagId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByListTags(List<int> tagIds, int pageNo, int elementEachPage);
        Task<Event?> GetEventById(Guid eventId);


        //Statistics
        Task<Dictionary<string, int>> CountByStatus();
        Task<List<EventPerMonth>> EventsPerMonth(DateTime startDate, DateTime endDate);
    }
}
