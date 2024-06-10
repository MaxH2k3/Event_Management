using Event_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.ParticipantDTO
{
	public class FilterParticipant
	{
		public int Page { get; set; } = 1;
		public int EachPage { get; set; } = 10;
		public Guid? EventId { get; set; }
		public int? RoleEventId { get; set; }
		public DateTime? CheckedIn { get; set; }
		public bool? IsCheckedMail { get; set; }
		public DateTime? CreatedAt { get; set; }
		public ParticipantSortBy SortBy { get; set; } = ParticipantSortBy.CreatedAt;
	}
}
