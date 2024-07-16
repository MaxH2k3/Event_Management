using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.UserDTO.Response
{
    public class TotalUserEachMonthDto
    {
        string? Month {  get; set; }
        int? Total { get; set; }
    }
}
