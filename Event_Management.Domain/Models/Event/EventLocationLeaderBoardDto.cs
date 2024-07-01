using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
    public class EventLocationLeaderBoardDto
    {
        public int totalevent { get; set; }
        public string Location {  get; set; }
        public string? LocationUrl { get; set; } = "";
        public string? LocationCoord {  get; set; } = "";
        public string? LocationAddress { get; set; } = "";
        public string? LocationId { get; set; } = "";
    }
}
