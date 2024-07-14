using Event_Management.Domain.Enum.Sponsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.SponsorDTO
{
	public class SponsorDto
	{
		public Guid EventId { get; set; }
		//public int? SponsorMethodId { get; set; }
		public Guid? UserId { get; set; }
		public SponsorRequest? Status { get; set; }
		public bool? IsSponsored { get; set; } = false;
		public decimal? Amount { get; set; }
		
		public DateTime? UpdatedAt { get; set; }
	}
}
