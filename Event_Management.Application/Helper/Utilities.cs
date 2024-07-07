using Event_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Helper
{
    public class Utilities
    {
        public static string GetTypeButton(int eventRole)
        {
            switch (eventRole)
            {
                case 1:
                case 3:
                    return "Approval";
                case 4:
                case 0:
                    return "My Ticket";
            }

            return string.Empty;
        }
    }
}
