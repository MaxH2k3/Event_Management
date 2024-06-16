using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class EventMailSystem
    {
        public Guid? EventId { get; set; }
        public DateTime? TimeExecute { get; set; }
        public string? MethodKey { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Type { get; set; }
        public Guid? CreatedBy { get; set; }

        public virtual Event? Event { get; set; }
    }
}
