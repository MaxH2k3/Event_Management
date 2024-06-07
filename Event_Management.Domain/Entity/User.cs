using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Domain
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Events = new HashSet<Event>();
            Feedbacks = new HashSet<Feedback>();
            Participants = new HashSet<Participant>();
            Payments = new HashSet<Payment>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? FirstName { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? LastName { get; set; }
        public int Age { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Gender { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Email { get; set; }
        [StringLength(15)]
        [Unicode(false)]
        public string? Phone { get; set; }
        public byte[]? Password { get; set; }
        public byte[] PasswordSalt { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? UpdatedAt { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Status { get; set; } = null!;
        [Column("RoleID")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; } = null!;
        [InverseProperty(nameof(Event.CreatedByNavigation))]
        public virtual ICollection<Event> Events { get; set; }
        [InverseProperty(nameof(Feedback.User))]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [InverseProperty(nameof(Participant.User))]
        public virtual ICollection<Participant> Participants { get; set; }
        [InverseProperty(nameof(Payment.User))]
        public virtual ICollection<Payment> Payments { get; set; }
        [InverseProperty(nameof(Transaction.User))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
