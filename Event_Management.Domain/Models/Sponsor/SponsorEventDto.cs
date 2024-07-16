using Event_Management.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Sponsor
{
    public class SponsorEventDto
    {
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public bool? IsSponsored { get; set; }
        public decimal? Amount { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
