using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
