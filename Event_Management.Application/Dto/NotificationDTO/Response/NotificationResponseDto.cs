using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.NotificationDTO.Response
{
    public class NotificationResponseDto
    {
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsRead { get; set; }
    }
}
