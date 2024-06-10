﻿using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class Logo
    {
        public Logo()
        {
            Events = new HashSet<Event>();
        }

        public int LogoId { get; set; }
        public string? SponsorBrand { get; set; }
        public string? LogoUrl { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
