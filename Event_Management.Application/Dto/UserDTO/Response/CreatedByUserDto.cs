using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.UserDTO.Response
{
    public class CreatedByUserDto
    {
        public string? Name { get; set; }
        public Guid? Id { get; set; }
        public string? avatar { get; set; }
    }
}
