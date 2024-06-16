using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
