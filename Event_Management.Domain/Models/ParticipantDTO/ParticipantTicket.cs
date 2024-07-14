using Event_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.ParticipantDTO
{
    public class ParticipantTicket
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
