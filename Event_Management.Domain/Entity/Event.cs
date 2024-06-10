﻿using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class Event
    {
        public Event()
        {
            Feedbacks = new HashSet<Feedback>();
            Participants = new HashSet<Participant>();
            Transactions = new HashSet<Transaction>();
            Logos = new HashSet<Logo>();
            Tags = new HashSet<Tag>();
        }

        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? Image { get; set; }
        public string? Location { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool? Approval { get; set; }
        public double? Ticket { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Logo> Logos { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
