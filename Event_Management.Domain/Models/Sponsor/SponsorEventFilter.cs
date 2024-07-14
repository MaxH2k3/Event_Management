using Event_Management.Domain.Enum.Sponsor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Sponsor
{
    public class SponsorEventFilter
    {
        [Required]
        public Guid EventId { get; set; }
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int EachPage { get; set; } = 10;
        public string? Status { get; set; }
        public bool? IsSponsored { get; set; }
    }
}
