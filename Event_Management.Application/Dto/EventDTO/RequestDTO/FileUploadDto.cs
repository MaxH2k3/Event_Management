using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.RequestDTO
{
    public class FileUploadDto
    {
        public string base64 { get; set; } = string.Empty;
        public Guid eventId { get; set; }
        public string sponsorName { get; set; } = string.Empty;
    }
}
