﻿using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class Tag
    {
        public Tag()
        {
            Events = new HashSet<Event>();
        }

        public int TagId { get; set; }
        public string? TagName { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
