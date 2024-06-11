using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.RequestDTO
{
    public class FileUploadDto
    {
        public string formFile { get; set; }
        public Guid eventId { get; set; }
    }
}
