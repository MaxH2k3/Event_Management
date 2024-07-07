using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Constants
{
    public class MailConstant
    {
        public class TicketMail
        {
            public const string Title = "Ticket for event";
            public const string PathTemplate = "Views/Template/TicketUser.cshtml";
        } 


    }
}
