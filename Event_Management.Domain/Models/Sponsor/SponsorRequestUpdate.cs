using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Sponsor
{
    public class SponsorRequestUpdate
    {
        public Guid EventId { get; set; }
        public string? Status { get; set; }
    }
}
