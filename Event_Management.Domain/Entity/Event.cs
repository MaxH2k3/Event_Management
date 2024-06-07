using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Event_Management.Domain
{
    [Table("Event")]
    public partial class Event
    {
        public Event()
        {
            Feedbacks = new HashSet<Feedback>();
            Packages = new HashSet<Package>();
            Participants = new HashSet<Participant>();
            Transactions = new HashSet<Transaction>();
            Tags = new HashSet<Tag>();
        }

        [Key]
        [Column("EventID")]
        public Guid EventId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string EventName { get; set; } = null!;
        [Unicode(false)]
        public string? Description { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string? Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        public Guid? CreatedBy { get; set; }

        [StringLength(5000)]
        [Unicode(false)]
        public string? Image { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Location { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedAt { get; set; }
        public int? Capacity { get; set; }
        public bool? Approval { get; set; }
        public double? Ticket { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.Events))]
        public virtual User? CreatedByNavigation { get; set; }
        [InverseProperty(nameof(Feedback.Event))]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [InverseProperty(nameof(Package.Event))]
        public virtual ICollection<Package> Packages { get; set; }
        [InverseProperty(nameof(Participant.Event))]
        public virtual ICollection<Participant> Participants { get; set; }
        [InverseProperty(nameof(Transaction.Event))]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("EventId")]
        [InverseProperty(nameof(Tag.Events))]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
