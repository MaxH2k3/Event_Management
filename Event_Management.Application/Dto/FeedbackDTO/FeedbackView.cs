using Event_Management.Application.Dto.UserDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.FeedbackDTO
{
    public class FeedbackView
    {
        public Guid EventId { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public CreatedByUserDto CreatedBy { get; set; } = new CreatedByUserDto();
    }
}
