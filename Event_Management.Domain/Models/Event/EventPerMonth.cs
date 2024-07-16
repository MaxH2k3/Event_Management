using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Models.Event
{
    public class EventPerMonth
    {
        public string month {  get; set; } = string.Empty;
        public int value { get; set; } = 0;
    }
}
