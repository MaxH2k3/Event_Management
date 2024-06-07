using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
	public class EventPreview
	{
		public Guid EventId { get; set; }
		public string EventName { get; set; } = null!;
		public string? Image { get; set; }
		public string? Location { get; set; }
		public double? Ticket { get; set; }
	}
}
