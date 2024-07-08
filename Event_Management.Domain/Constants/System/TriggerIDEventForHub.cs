using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Constants
{
    public class TriggerIDEventForHub
    {

        public class CheckinUser
        {
            public const string TriggerEvent = "CheckinUser(Guid userId, Guid eventId)";
            public const string Description = "Triggers the check-in process for a user at an event. Sends a notification with the check-in result, indicating whether the check-in was successful or failed.";
        }

        public class StatictisParticipant
        {
            public const string TriggerEvent = "StatictisParticipant(Guid eventId)";
            public const string Description = "Triggers the process of calculating the number of participants who have checked in at an event. Sends a notification with the number of participants who have checked in.";
        }

    }
}
