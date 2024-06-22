﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Message
{
    public static class MessageEvent
    {
        //Event Message response
        public const string StartEndTimeValidation = "Start date must after current time and end date must after start date 30 mins!!";
        public const string EventIdNotExist = "EventId not exist!";
        public const string GetAllEvent = "Get all events!";
        public const string UserParticipatedEvent = "User participated events!";
    }
}