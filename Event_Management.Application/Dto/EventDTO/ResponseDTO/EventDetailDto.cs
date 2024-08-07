﻿using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Dto.SponsorLogoDTO;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.ParticipantDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
    public class EventDetailDto
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
        public EventLocation? location { get; set; } = new EventLocation();
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool? Approval { get; set; }
        public decimal? Fare { get; set; }
        public List<SponsorLogoDto> sponsorLogos { get; set; } = new List<SponsorLogoDto>();
    }
}
