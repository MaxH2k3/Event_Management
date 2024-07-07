using Event_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.ParticipantDTO
{
	public class FilterParticipant
	{
		[Range(1, int.MaxValue)]
		public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int EachPage { get; set; } = 10;
		[Required]
		public Guid EventId { get; set; }
		[Range(1, 4)]
		public int? RoleEventId { get; set; }
		public DateTime? CheckedIn { get; set; }
		public bool? IsCheckedMail { get; set; }
		public DateTime? CreatedAt { get; set; }
		public ParticipantSortBy SortBy { get; set; } = ParticipantSortBy.CreatedAt;
	}
}
