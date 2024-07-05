using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.ParticipantDTO
{
    public class TicketModel
    {
        public string? OrgainzerName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public byte[]? TicketQR { get; set; }
        public string? LogoEvent { get; set; }
        public string? EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
    }
}
