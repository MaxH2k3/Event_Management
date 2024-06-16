using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class Feedback
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
