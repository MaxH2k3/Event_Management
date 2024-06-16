using System;
using System.Collections.Generic;

namespace Event_Management.Domain.Entity
{
    public partial class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public Guid? UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

        public virtual User? User { get; set; }
    }
}
