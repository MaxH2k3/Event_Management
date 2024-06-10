using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class SponsorEvent
    {
        public Guid? EventId { get; set; }
        public int? SponsorMethodId { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Event? Event { get; set; }
        public virtual SponsorMethod? SponsorMethod { get; set; }
        public virtual User? User { get; set; }
    }
}
