﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.EventDTO.ResponseDTO
{
    public class EventCreatorLeaderBoardDto
    {
        public Guid? userId { get; set; }
        public string? FullName { get; set; } = "";
        public string? Avatar {  get; set; } = "";
        public int totalevent {  get; set; }
    }
}
