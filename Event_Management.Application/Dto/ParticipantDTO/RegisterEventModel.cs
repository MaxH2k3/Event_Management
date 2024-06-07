﻿using System.ComponentModel.DataAnnotations;

namespace Event_Management.Application.Dto.ParticipantDTO
{
	public class RegisterEventModel
	{
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public Guid EventId { get; set; }
		[Required]
		[Range(1, 4)]
		public int RoleEventId { get; set; }
	}
}
