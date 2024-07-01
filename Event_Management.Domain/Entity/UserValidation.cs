using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class UserValidation
    {
        public Guid UserId { get; set; }
        public string? Otp { get; set; }
        public string? VerifyToken { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
