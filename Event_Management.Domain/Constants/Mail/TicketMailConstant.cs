using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Constants
{
    public class TicketMailConstant
    {
        public static IEnumerable<string> MessageMail = new List<string>
        {
            "You have registered for",
            "You are an Sponsor for",
            "You are an Event Operator for",
            "You are an Checking Staff for",
            "You have registered for"
        };
    }
}
