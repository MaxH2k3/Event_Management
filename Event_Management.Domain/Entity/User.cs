using System;
using System.Collections.Generic;

namespace Event_Management.Domain
{
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            Participants = new HashSet<Participant>();
            Payments = new HashSet<Payment>();
            RefreshTokens = new HashSet<RefreshToken>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public byte[]? Password { get; set; }
        public byte[] PasswordSalt { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
