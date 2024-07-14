using Event_Management.Application.Dto.UserDTO.Response;

namespace Event_Management.Domain.Models.EventDTO.ResponseDTO
{
    public class EventPreview
	{
		public Guid EventId { get; set; }
		public string EventName { get; set; } = null!;
        public string? Status { get; set; }
        public long StartDate { get; set; }
        public CreatedByUserDto? Host { get; set; } = new CreatedByUserDto();
        public string? Image { get; set; }
		public string? Location { get; set; }
	}
}
