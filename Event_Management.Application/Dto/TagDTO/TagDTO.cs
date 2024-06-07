using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto
{
    public class TagDTO
    {
        //public int TagId { get; set; }
        public string TagName { get; set; }
        public Guid EventId { get; set; }
    }
}
