using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Message
{
    public static class MessageEvent
    {
        //Event Message response
        public const string StartEndTimeValidation = "Start date must after current time 12 hours and end date must after start date 30 mins!!";
        public const string EventIdNotExist = "EventId not exist!";
        public const string GetAllEvent = "Get all events!";
        public const string UserParticipatedEvent = "User participated events!";
        public const string PopularLocation = "Popular locations leaderboard";
        public const string PopularOrganizers = "Popular organizers(event creators)";
        public const string UserNotAllow = "User not allowed to create event";
        public const string LocationCoordInvalid = "Location coordinate must follow pattern: @\"^-?\\d+(?:\\.\\d+)?, *-?\\d+(?:\\.\\d+)?$\"";
        public const string TagLimitValidation = "Event's maximum tags is 5!";
        public const string UpdateEventWithStatus = "Can only update Event with status NotYet";
        public const string OnlyHostCanUpdateEvent = "Only host can update this event";
    }
}
