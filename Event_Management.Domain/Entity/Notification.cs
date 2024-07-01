using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class Notification
    {
        public int NotificationIdD { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsRead { get; set; }

        public virtual User? User { get; set; }
    }
}
