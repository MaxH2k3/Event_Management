﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
    public class ParticipantInfo
    {
        public Guid UserId { get; set; }
        public int? RoleEventId { get; set; }
        public string? Status { get; set; }
    }
}
