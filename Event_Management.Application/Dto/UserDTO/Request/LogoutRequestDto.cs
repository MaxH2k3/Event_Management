﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Dto.UserDTO.Request
{
    public class LogoutRequestDto
    {
        public string? RefreshToken { get; set; }
    }
}
