﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.ParticipantDTO
{
	public class ParticipantEventModel
	{
		public Guid UserId { get; set; }
		public int? RoleEventId { get; set; }
		public DateTime? CheckedIn { get; set; }
		public bool? IsCheckedMail { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Status { get; set; }

	}
}
