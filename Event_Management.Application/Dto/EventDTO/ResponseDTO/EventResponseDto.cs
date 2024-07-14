using Event_Management.Application.Dto.UserDTO.Response;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
    public class EventResponseDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public List<EventTag> eventTags { get; set; } = new List<EventTag>();
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public CreatedByUserDto? Host { get; set; } = new CreatedByUserDto();
        public string? Image { get; set; }
        public string? Theme { get; set; }
        public EventLocation? Location { get; set; } = new EventLocation();
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool? Approval { get; set; }
        public decimal? Fare { get; set; }
    }
}
